using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Application.NewItems.Dtos;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.NewItems.Queries;
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
