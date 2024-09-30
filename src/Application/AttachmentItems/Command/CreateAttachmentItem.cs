using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AlphaVisa.Application.AttachmentItems.Command;
public record CreateAttachmentItemBriefDto(string? Name, string? Mimetype, long? Size, string? Url) : IAuditableDto
{
    public DateTimeOffset? Created {  get; set; }
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
        var uploadPath = Path.Combine("FileStores", request.File.FileName);
        Directory.CreateDirectory(Path.GetDirectoryName(uploadPath)!);
        using var fileStream = new FileStream(uploadPath, FileMode.Create);
        using var inputStream = request.File.OpenReadStream();
        await inputStream.CopyToAsync(fileStream);

        var hostUrl = _hostService.GetHostUrl();

        var attachmentItem = new AttachmentItem()
        {
            ObjectId = objectId,
            Name = request.File.FileName,
            NameWithoutExtension = Path.GetFileNameWithoutExtension(request.File.FileName),
            MimeType = "application/octet-stream",
            Url = $"{hostUrl}/api/v1/AttachmentItems/{objectId}",
            SizeBytes = inputStream.Length
        };

        _context.AttachmentItems.Add(attachmentItem);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAttachmentItemBriefDto(attachmentItem.Name, attachmentItem.MimeType, attachmentItem.SizeBytes, attachmentItem.Url)
        {
            Created = attachmentItem.Created,
            LastModified = attachmentItem.LastModified,
        };
    }
}
