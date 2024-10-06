using System.IO;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Domain.Enums;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace AlphaVisa.Application.AttachmentItems.Command;
public record CreateAttachmentItemBriefDto(string? FileName, string? MimeType, long? Size, string? Url, string? ThumbnailUrl, FileSource? Source) : IAuditableDto
{
    public DateTimeOffset? CreatedAt {  get; set; }
    public DateTimeOffset? LastModified {  get; set; }
}

public record CreateAttachmentItem(IFormFile File) : IRequest<CreateAttachmentItemBriefDto>;

public class CreateAttachmentItemValidator : AbstractValidator<CreateAttachmentItem>
{
    public CreateAttachmentItemValidator(ISharedLocalizer localizer)
    {
        RuleFor(f => f.File)
           .NotEmpty();
    }
}

public class CreateAttachmentItemHandler : IRequestHandler<CreateAttachmentItem, CreateAttachmentItemBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IHostService _hostService;

    public CreateAttachmentItemHandler(IApplicationDbContext context, IHostService hostService)
    {
        _context = context;
        _hostService = hostService;
    }

    public async Task<CreateAttachmentItemBriefDto> Handle(CreateAttachmentItem request, CancellationToken cancellationToken)
    {
        var objectId = Guid.NewGuid();
        var uploadPath = Path.Combine("filestores", objectId.ToString());
        var thumbnailName = $"{objectId}_thumbnail";
        var thumbnailPath = Path.Combine("filestores", thumbnailName);

        // Save the original file
        Directory.CreateDirectory(Path.GetDirectoryName(uploadPath)!);
        using var fileStream = new FileStream(uploadPath, FileMode.Create);
        using var inputStream = request.File.OpenReadStream();
        await inputStream.CopyToAsync(fileStream);

        // Save the thumbnail file
        inputStream.Seek(0, SeekOrigin.Begin);
        using var thumbnailStream = new FileStream(thumbnailPath, FileMode.Create);
        using var image = await Image.LoadAsync(inputStream, cancellationToken);
        image.Mutate((ctx) =>
        {
            ctx.Resize(new ResizeOptions()
            {
                Size = new Size(150, 150),
                Mode = ResizeMode.Crop
            });
        });

        // Save the thumbnail file with reduced quality to reduce disk size
        var jpegEncoder = new JpegEncoder
        {
            Quality = 50 // Reduce quality to approximately 50% to reduce file size on disk
        };

        await image!.SaveAsync(thumbnailStream, jpegEncoder, cancellationToken);


        var hostUrl = _hostService.GetHostUrl();

        var attachmentItem = new AttachmentItem()
        {
            ObjectId = objectId,
            Name = request.File.FileName,
            NameWithoutExtension = Path.GetFileNameWithoutExtension(request.File.FileName),
            MimeType = "application/octet-stream",
            Url = $"{hostUrl}/api/v1/AttachmentItems/{objectId}",
            ThumbnailUrl = $"{hostUrl}/api/v1/AttachmentItems/{objectId}?isThumbnail=true",
            Source = FileSource.Local,
            SizeBytes = inputStream.Length
        };

        _context.AttachmentItems.Add(attachmentItem);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAttachmentItemBriefDto(attachmentItem.Name, 
            attachmentItem.MimeType, 
            attachmentItem.SizeBytes, 
            attachmentItem.Url, 
            attachmentItem.ThumbnailUrl, 
            attachmentItem.Source)
        {
            CreatedAt = attachmentItem.CreatedAt,
            LastModified = attachmentItem.LastModified,
        };
    }
}
