using AlphaVisa.Domain.Entities;

namespace AlphaVisa.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<ServiceItem> ServiceItems { get; }

    DbSet<NewItem> NewItems { get; }

    DbSet<ContactItem> ContactItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
