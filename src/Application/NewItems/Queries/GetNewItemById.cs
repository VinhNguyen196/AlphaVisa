using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.NewItems.Dtos;

namespace AlphaVisa.Application.NewItems.Queries;
public record GetNewItemByIdQuery(Guid Id) : IRequest<NewItemBriefDto>;

public class GetNewItemByIdQueryHandler : IRequestHandler<GetNewItemByIdQuery, NewItemBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNewItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NewItemBriefDto> Handle(GetNewItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.NewItems
            .Include(x => x.Thumbnail)
            .ProjectTo<NewItemBriefDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return entity!;
    }
}
