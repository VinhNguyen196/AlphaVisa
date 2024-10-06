using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AlphaVisa.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;
    private readonly TimeProvider _dateTime;

    public AuditableEntityInterceptor(
        IUser user,
        TimeProvider dateTime)
    {
        _user = user;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = _dateTime.GetUtcNow();
        var genericBaseEntTypeDef = typeof(BaseAuditableEntity<>);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            var entType = entry.Entity.GetType();
            var baseType = entType.BaseType;

            if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericBaseEntTypeDef)
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var auditableEntity = entry.Entity as dynamic;  // Cast to dynamic to access properties

                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedBy = _user.Id;
                        auditableEntity.CreatedAt = utcNow;
                    }

                    auditableEntity.LastModifiedBy = _user.Id;
                    auditableEntity.LastModified = utcNow;
                }
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
