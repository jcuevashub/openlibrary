using System.Text;
using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;

namespace OpenLibrary.Application.Services;

public class BookService : IBookService
{
    private readonly ICacheService<BookDto> _cacheService;
    private readonly IOpenLibraryService _openLibraryService;
    string bookLocation = "";
    public BookService(ICacheService<BookDto> cacheService, IOpenLibraryService openLibraryService)
    {
        _cacheService = cacheService;
        _openLibraryService = openLibraryService;
    }

    public async Task<BookDto> GetBookInfoAsync(string isbn)
    {
        if (!_cacheService.TryGetValue(isbn, out var bookDto))
        {
            bookDto = await FetchAndCacheBookDataAsync(isbn);
        }
        return bookDto;
    }

    public async Task<string> GetBooksInfoAsync(string filePath, List<string> isbns)
    {
        var bookDtos = new List<BookDto>();

        var customLocation = Path.Combine(filePath, "isbn_list.csv");

        var csv = new StringBuilder("Row Number;Data Retrieval Type;ISBN;Title;Subtitle;Author Name(s);Number of Pages;Publish Date\n");
        int rowNumber = 1;

        foreach (var isbn in isbns)
        {
            var bookInfo = await FetchAndCacheBookDataAsync(isbn);
            if (bookInfo != null)
            {
                csv.AppendLine($"{rowNumber};{bookLocation};{isbn};{bookInfo.Title};{bookInfo.Subtitle};{bookInfo.Authors};{bookInfo.NumberOfPages};{bookInfo.PublishDate}");
                rowNumber++;
                bookDtos.Add(bookInfo);
            }
        }

        await File.WriteAllTextAsync(customLocation, csv.ToString());
        return csv.ToString();
    }

    public async Task<string> GetBooksInfoFromFileAsync(string filePath, IFormFile file)
    {
        var customLocation = Path.Combine(filePath, "isbn_list.csv");

        var csv = new StringBuilder("Row Number;Data Retrieval Type;ISBN;Title;Subtitle;Author Name(s);Number of Pages;Publish Date\n");
        int rowNumber = 1;

        using var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        var fileContent = await streamReader.ReadToEndAsync();
        var isbns = ExtractIsbns(fileContent);

        foreach (var isbn in isbns)
        {
            var bookInfo = await FetchAndCacheBookDataAsync(isbn);
            if (bookInfo != null)
            {
                csv.AppendLine($"{rowNumber};{"Server"};{isbn};{bookInfo.Title};{bookInfo.Subtitle};{bookInfo.Authors};{bookInfo.NumberOfPages};{bookInfo.PublishDate}");
                rowNumber++;
            }
        }

        await File.WriteAllTextAsync(customLocation, csv.ToString());
        return csv.ToString();
    }

    private async Task<BookDto> FetchAndCacheBookDataAsync(string isbn)
    {
        if (_cacheService.TryGetValue(isbn, out var bookDto))
        {
            bookLocation = "Cache";
            return bookDto;
        }

        bookDto = await _openLibraryService.FetchBookDataAsync(isbn);
        if (bookDto != null)
        {
            bookLocation = "Server";
            _cacheService.Set(isbn, bookDto);
        }

        return bookDto;
    }


    private static string[] ExtractIsbns(string content)
    {
        var rows = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        var isbns = rows.SelectMany(row => row.Split(','));
        return isbns.ToArray();
    }

}