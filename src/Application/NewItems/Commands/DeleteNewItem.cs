using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaVisa.Application.Common.Interfaces;

namespace AlphaVisa.Application.NewItems.Commands;
public record DeleteNewItem(Guid Id) : IRequest;

public class DeleteNewItemHandler : IRequestHandler<DeleteNewItem>
{
    private readonly IApplicationDbContext _context;

    public DeleteNewItemHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteNewItem request, CancellationToken cancellationToken)
    {
        var entity = await _context.NewItems
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.NewItems.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
