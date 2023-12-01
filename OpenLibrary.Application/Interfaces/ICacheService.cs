namespace OpenLibrary.Application.Interfaces;

public interface ICacheService<T>
{
    T? Get(string key);
    void Set(string key, T item);
    bool TryGetValue(string key, out T? value);
}
