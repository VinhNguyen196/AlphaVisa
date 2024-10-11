using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Application.ContactItems.Dtos;

namespace AlphaVisa.Application.ContactItems.Queries;
public record GetContactItemsWithPaginationQuery : IRequest<PaginatedList<ContactItemBriefDto>>
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}

public class GetContactItemsWithPaginationQueryValidator : AbstractValidator<GetContactItemsWithPaginationQuery>
{
    public GetContactItemsWithPaginationQueryValidator(ISharedLocalizer localizer)
    {
        RuleFor(ci => ci.PageSize)
            .GreaterThanOrEqualTo(localizer);

        RuleFor(ci => ci.PageNumber)
            .GreaterThanOrEqualTo(localizer);
    }
}

public class GetContactItemsWithPaginationQueryHandler : IRequestHandler<GetContactItemsWithPaginationQuery, PaginatedList<ContactItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ContactItemBriefDto>> Handle(GetContactItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ContactItems
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.Thumbnail)
            .ProjectTo<ContactItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

