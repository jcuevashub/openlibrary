using OpenLibrary.Application.Dtos;

namespace OpenLibrary.Application.Interfaces; 

public interface IBookService
{
    Task<BookDto> GetBookInfoAsync(string isbn);
}
