using OpenLibrary.Domain.Entities;

namespace OpenLibrary.Domain.Interfaces;

public interface IBookRepository
{
    Task<Book> GetBookISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetBookAsync();
}
