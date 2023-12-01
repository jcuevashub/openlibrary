using OpenLibrary.Domain.Entities;
using OpenLibrary.Domain.Interfaces;

namespace OpenLibrary.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    public Task<IEnumerable<Book>> GetBookAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Book> GetBookISBNAsync(string isbn)
    {
        throw new NotImplementedException();
    }
}
