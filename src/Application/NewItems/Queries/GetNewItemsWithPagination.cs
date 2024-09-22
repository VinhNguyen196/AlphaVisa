﻿using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.NewItems.Queries;
public record NewItemBriefDto : IAuditableDto
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
            CreateMap<NewItem, NewItemBriefDto>();
        }
    }
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
            .OrderBy(x => x.Topic)
            .ProjectTo<NewItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
