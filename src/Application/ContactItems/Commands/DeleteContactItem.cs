using AlphaVisa.Application.Common.Interfaces;

namespace AlphaVisa.Application.ContactItems.Commands;
public record DeleteContactItem(Guid Id) : IRequest;

public class DeleteContactItemHandler : IRequestHandler<DeleteContactItem>
{
    private readonly IApplicationDbContext _context;

    public DeleteContactItemHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteContactItem request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactItems
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.ContactItems.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

