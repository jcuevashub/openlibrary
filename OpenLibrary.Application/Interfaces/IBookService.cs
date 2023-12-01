using Microsoft.AspNetCore.Http;

namespace OpenLibrary.Application.Interfaces;

public interface IBookService
{
    Task<string> GetBooksInfoFromFileAsync(IFormFile file);
}
