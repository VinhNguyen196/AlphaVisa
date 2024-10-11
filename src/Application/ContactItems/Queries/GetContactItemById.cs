using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.ContactItems.Dtos;

namespace AlphaVisa.Application.ContactItems.Queries;
public record GetContactItemByIdQuery(Guid Id) : IRequest<ContactItemBriefDto>;

public class GetContactItemByIdQueryHandler : IRequestHandler<GetContactItemByIdQuery, ContactItemBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactItemBriefDto> Handle(GetContactItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactItems
            .Include(x => x.Thumbnail)
            .ProjectTo<ContactItemBriefDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return entity!;
    }
}
