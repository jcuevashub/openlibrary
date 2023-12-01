using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Dtos;

namespace OpenLibrary.Application.Interfaces;

public interface IBookService
{
    Task<string> GetBooksInfoFromFileAsync(IFormFile file);
}
