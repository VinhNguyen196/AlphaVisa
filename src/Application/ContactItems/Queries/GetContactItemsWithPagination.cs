using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Mappings;
using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.Common.Validators;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.ContactItems.Queries;
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

