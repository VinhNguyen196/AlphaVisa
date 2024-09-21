using System.Collections.Concurrent;

namespace AlphaVisa.Application.Common.Caches;

// Interface that defines the contract for caching entities
public interface IEntityCache
{
    void SetEntity<T>(T entity) where T : class;
    T GetEntity<T>() where T : class;
    void ClearEntity();
}

// Abstract base class that provides the caching functionality for commands
public abstract class EntityCacheBase : IEntityCache
{
    // Use a private dictionary to store cached entities per command instance
    private readonly ConcurrentDictionary<string, object> _entityCache = new();

    // Store the entity in the cache with a string key
    public void SetEntity<T>(T entity) where T : class
    {
        var key = typeof(T).FullName ?? "Entity";
        _entityCache[key] = entity;
    }

    // Retrieve the cached entity from the dictionary
    public T GetEntity<T>() where T : class
    {
        var key = typeof(T).FullName ?? "Entity";
        if (_entityCache.TryGetValue(key, out var entity))
        {
            return entity as T ?? throw new InvalidOperationException($"Entity of type {typeof(T)} not found.");
        }
        throw new InvalidOperationException($"Entity of type {typeof(T)} not found in cache.");
    }

    // Clear the cached entity
    public void ClearEntity()
    {
        _entityCache.Clear();
    }
}
