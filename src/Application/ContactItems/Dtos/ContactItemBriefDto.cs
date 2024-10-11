using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.ContactItems.Dtos;
public record ContactItemBriefDto : IAuditableDto, IMapFrom<ContactItem>
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public AttachmentItemDtoBrief? Thumbnail { get; set; }

    public string? Story { get; set; }

    public ComplextObject? Metadata { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
