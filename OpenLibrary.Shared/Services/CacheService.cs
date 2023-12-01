using OpenLibrary.Application.Interfaces;
using System.Collections.Concurrent;

namespace OpenLibrary.Shared.Services;

public class CacheService<T> : ICacheService<T> where T : class
{
    private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();

    public T? Get(string key)
    {
       return _cache.TryGetValue(key, out var item) ? item : default;
    }

    public void Set(string key, T item)
    {
        _cache[key] = item;
    }

    public bool TryGetValue(string key, out T? value)
    {
        return _cache.TryGetValue(key, out value);
    }
}
