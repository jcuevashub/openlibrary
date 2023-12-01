using OpenLibrary.Application.Dtos;

namespace OpenLibrary.Application.Interfaces;

public interface IOpenLibraryService
{
    Task<BookDto> FetchBookDataAsync(string isbn);
}
