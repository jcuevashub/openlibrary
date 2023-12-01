using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;
using OpenLibrary.Domain.Interfaces;

namespace OpenLibrary.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;

    }

    public async Task<BookDto> GetBookInfoAsync(string isbn)
    {
        var result =  await _bookRepository.GetBookISBNAsync(isbn);


        return result;
    }
    
}
