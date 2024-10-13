using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.ContactItems.Dtos;
public record AttachmentItemDtoBrief : IAuditableDto, IMapFrom<AttachmentItem>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttachmentItem, AttachmentItemDtoBrief>()
            .ForMember(dest => dest.FileName, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.Size, opts => opts.MapFrom(src => src.SizeBytes));
    }

    public Guid? Id { get; set; }

    public string? FileName { get; set; }

    public string? NameWithoutExtension { get; set; }

    public string? MimeType { get; set; }

    public Guid? ObjectId { get; set; }

    public string? Url { get; set; }

    public string? ThumbnailUrl {  get; set; }

    public long? Size { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
