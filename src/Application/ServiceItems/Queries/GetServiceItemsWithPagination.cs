using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Domain.Enums;

namespace AlphaVisa.Application.ServiceItems.Queries;

public record ServiceItemBriefDto : IAuditableDto
{
    public Guid? Id { get; set; }

    public ServiceItemType? Type { get; set; }

    public string? Name { get; set; }

    public string? Thumbnail { get; set; }

    public ComplextObject? Description { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ServiceItem, ServiceItemBriefDto>();
        }
    }
};

public record GetServiceItemsWithPaginationQuery : IRequest<PaginatedList<ServiceItemBriefDto>>
{
    public ServiceItemType? Type { get; set; }

    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
};


public class GetServiceItemsWithPaginationQueryValidator : AbstractValidator<GetServiceItemsWithPaginationQuery>
{
    public GetServiceItemsWithPaginationQueryValidator(ISharedLocalizer localizer)
    {
        RuleFor(si => si.Type)
            .IsInEnum()
            .When(si => si.Type.HasValue);

        RuleFor(sis => sis.PageSize)
            .GreaterThanOrEqualTo(localizer);

        RuleFor(sis => sis.PageNumber)
            .GreaterThanOrEqualTo(localizer);
    }
}

public class GetServiceItemsWithPaginationQueryHandler : IRequestHandler<GetServiceItemsWithPaginationQuery, PaginatedList<ServiceItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServiceItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ServiceItemBriefDto>> Handle(GetServiceItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.ServiceItems.AsQueryable();

        if (request.Type is not null)
        {
            queryable = queryable.Where(x => x.Type == request.Type);
        }

        return await queryable.OrderByDescending(x => x.Created)
           .ProjectTo<ServiceItemBriefDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
