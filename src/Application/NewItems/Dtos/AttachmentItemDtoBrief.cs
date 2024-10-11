using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.NewItems.Dtos;
public record AttachmentItemDtoBrief : IAuditableDto, IMapFrom<AttachmentItem>
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public string? NameWithoutExtension { get; set; }

    public string? MimeType { get; set; }

    public Guid? ObjectId { get; set; }

    public string? Url { get; set; }

    public long? SizeBytes { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
