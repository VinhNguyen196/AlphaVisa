using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Application.ServiceItems.Queries;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.ContactItems.Queries;
public record ContactItemBriefDto : IAuditableDto, IMapFrom<ContactItem>
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public string? Thumbnail { get; set; }

    public string? Story { get; set; }

    public ComplextObject? Metadata { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}

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
            .ProjectTo<ContactItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

