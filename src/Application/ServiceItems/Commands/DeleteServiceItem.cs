using AlphaVisa.Application.Common.Interfaces;

namespace AlphaVisa.Application.ServiceItems.Commands;
public record DeleteServiceItemCommand(Guid Id) : IRequest;

public class DeleteServiceItemCommandHandler : IRequestHandler<DeleteServiceItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteServiceItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteServiceItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ServiceItems
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.ServiceItems.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
