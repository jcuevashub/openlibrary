using Newtonsoft.Json;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibrary.Shared.Services;

public class OpenLibraryService : IOpenLibraryService
{
    private readonly HttpClient _httpClient;

    public OpenLibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BookDto> FetchBookDataAsync(string isbn)
    {
        var reponseString = await _httpClient.GetStringAsync($"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data");

        var responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reponseString);

        if (responseData.ContainsKey($"ISBN:{isbn}"))
        {
            var bookData = responseData[$"ISBN:{isbn}"];

            var bookDto = new BookDto
            {
                ISBN = isbn,
                Title = bookData.Title,
                Subtitle = bookData.Subtitle,
                Authors = bookData.Authors,
                PublishDate = bookData.PublishDate
            };

            return bookDto;
        }

        return null;
    }
}
