using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.AttachmentItems.Query;

public record AttachmentItemDtoBrief : IAuditableDto, IMapFrom<AttachmentItem>
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public string? NameWithoutExtension {  get; set; }

    public string? MimeType { get; set; }

    public Guid? ObjectId { get; set; }

    public string? Url { get; set; }

    public long? SizeBytes { get; set; }

    public DateTimeOffset? CreatedAt {  get; set; }

    public DateTimeOffset? LastModified {  get; set; }
}

public record GetAttachmentItemByObjectIdQuery(Guid ObjectId) : IRequest<AttachmentItemDtoBrief>;

public class GetAttachmentItemByObjectIdQueryValidator : AbstractValidator<GetAttachmentItemByObjectIdQuery>
{
    public GetAttachmentItemByObjectIdQueryValidator()
    {
        RuleFor(a => a.ObjectId)
            .NotEmpty();
    }
}

public class GetAttachmentItemByObjectIdQueryHandler : IRequestHandler<GetAttachmentItemByObjectIdQuery, AttachmentItemDtoBrief>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAttachmentItemByObjectIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AttachmentItemDtoBrief> Handle(GetAttachmentItemByObjectIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AttachmentItems
            .SingleOrDefaultAsync(x => x.ObjectId == request.ObjectId, cancellationToken);

        Guard.Against.NotFound(request.ObjectId, entity);

        var returnedValue = _mapper.Map<AttachmentItem, AttachmentItemDtoBrief>(entity);

        return returnedValue;
    }
}
