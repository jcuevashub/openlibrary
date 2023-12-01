using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Dtos;

namespace OpenLibrary.Application.Interfaces;

public interface IBookService
{
    Task<BookDto> GetBookInfoAsync(string isbns);

    Task<string> GetBooksInfoAsync(string filePath,List<string> isbns);
    Task<string> GetBooksInfoFromFileAsync(string filePath,IFormFile file);
}
