using Newtonsoft.Json;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;

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
        string apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";

        try
        {
            var responseString = await _httpClient.GetStringAsync(apiUrl);

            var responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseString);

            if (responseData?.Count > 0)
            {
                var firstElement = responseData.First();
                var bookData = firstElement.Value;

                var bookDto = MapToBookDto(isbn, bookData);

                return bookDto;
            }
            else if (responseData?.ContainsKey($"ISBN:{isbn}") == true)
            {
                var bookData = responseData[$"ISBN:{isbn}"];

                var bookDto = MapToBookDto(isbn, bookData);

                return bookDto;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching book data for ISBN {isbn}: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing book data for ISBN {isbn}: {ex.Message}");
        }
        return null;
    }

    private BookDto MapToBookDto(string isbn, dynamic bookData)
    {
        return new BookDto
        {
            ISBN = isbn,
            Title = bookData.title,
            Subtitle = bookData.subtitle,
            Authors = string.Join(", ", (bookData.authors as IEnumerable<dynamic>).Select(author => (string)author.name).ToList()),
            NumberOfPages = bookData.number_of_pages,
            PublishDate = bookData.publish_date
        };
    }
}
