using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.NewItems.Queries;
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

public record NewItemBriefDto : IAuditableDto, IMapFrom<NewItem>
{
    public Guid? Id { get; set; }

    public string? Topic { get; set; }

    public AttachmentItemDtoBrief? Thumbnail { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public ComplextObject? Content { get; set; }

    public DateTimeOffset? CreatedAt { get; set ; }

    public DateTimeOffset? LastModified { get; set; }
}

public record GetNewItemsWithPaginationQuery : IRequest<PaginatedList<NewItemBriefDto>>
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}

public class GetNewItemsWithPaginationQueryValidator : AbstractValidator<GetNewItemsWithPaginationQuery>
{
    public GetNewItemsWithPaginationQueryValidator(ISharedLocalizer localizer)
    {
        RuleFor(ni => ni.PageSize)
            .GreaterThanOrEqualTo(localizer);

        RuleFor(ni => ni.PageNumber)
            .GreaterThanOrEqualTo(localizer);
    }
}

public class GetNewItemsWithPaginationQueryHandler : IRequestHandler<GetNewItemsWithPaginationQuery, PaginatedList<NewItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNewItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewItemBriefDto>> Handle(GetNewItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.NewItems
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.Thumbnail)
            .ProjectTo<NewItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
