using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.NewItems.Dtos;
public record NewItemBriefDto : IAuditableDto, IMapFrom<NewItem>
{
    public Guid? Id { get; set; }

    public string? Topic { get; set; }

    public AttachmentItemDtoBrief? Thumbnail { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public ComplextObject? Content { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
