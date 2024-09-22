using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Application.ServiceItems.Queries;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.NewItems.Queries;
public record NewsItemBriefDto : IAuditableDto
{
    public Guid? Id { get; set; }

    public string? Topic { get; set; }

    public string? Thumbnail { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public ComplextObject? Content { get; set; }

    public DateTimeOffset? Created { get; set ; }

    public DateTimeOffset? LastModified { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<NewItem, NewsItemBriefDto>();
        }
    }
}

public record GetNewItemsWithPaginationQuery : IRequest<PaginatedList<NewsItemBriefDto>>
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}

public class GetServiceItemsWithPaginationQueryValidator : AbstractValidator<GetServiceItemsWithPaginationQuery>
{
    public GetServiceItemsWithPaginationQueryValidator(ISharedLocalizer localizer)
    {
        RuleFor(sis => sis.PageSize)
            .GreaterThanOrEqualTo(localizer);

        RuleFor(sis => sis.PageNumber)
            .GreaterThanOrEqualTo(localizer);
    }
}

public class GetNewItemsWithPaginationQueryHandler : IRequestHandler<GetNewItemsWithPaginationQuery, PaginatedList<NewsItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNewItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewsItemBriefDto>> Handle(GetNewItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.NewItems
            .OrderBy(x => x.Topic)
            .ProjectTo<NewsItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
