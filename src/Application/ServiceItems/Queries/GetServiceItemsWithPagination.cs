using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Domain.Enums;

namespace AlphaVisa.Application.ServiceItems.Queries;

public class ServiceItemBriefDto
{
    public string? Id { get; set; }
    public ServiceItemType? Type { get; set; }
    public string? Name { get; set; }
    public string? Thumbnail { get; set; }
    public ComplextObject? Description { get; set; }

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
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
};


public class GetServiceItemsWithPaginationQueryValidator : AbstractValidator<GetServiceItemsWithPaginationQuery>
{
    public GetServiceItemsWithPaginationQueryValidator(ISharedLocalizer localizer)
    {
        RuleFor(sis => sis.PageSize)
            .GreaterOrEqualTo(localizer);

        RuleFor(sis => sis.PageNumber)
            .GreaterOrEqualTo(localizer);
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
        return await _context.ServiceItems
           .OrderBy(x => x.Name)
           .ProjectTo<ServiceItemBriefDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
