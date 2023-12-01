using System.Text;
using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;
using OpenLibrary.Domain;

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

    public async Task<string> GetBooksInfoFromFileAsync( IFormFile file)
    {
        var csv = new StringBuilder("Row Number;Data Retrieval Type;ISBN;Title;Subtitle;Author Name(s);Number of Pages;Publish Date\n");
        int rowNumber = 1;

        var isbns = await ProcessUniqueNumbersAsync(file);

        foreach (var isbn in isbns)
        {
            var bookInfo = await FetchAndCacheBookDataAsync(isbn);
            if (bookInfo != null)
            {
                csv.AppendLine($"{rowNumber};{bookLocation};{isbn};{bookInfo.Title};{bookInfo.Subtitle};{bookInfo.Authors};{bookInfo.NumberOfPages};{bookInfo.PublishDate}");
                rowNumber++;
            }
        }

        return csv.ToString();
    }

    private async Task<BookDto> FetchAndCacheBookDataAsync(string isbn)
    {
        if (_cacheService.TryGetValue(isbn, out var bookDto))
        {
            bookLocation = BookLocation.Cache.GetDescription();
            return bookDto;
        }

        bookDto = await _openLibraryService.FetchBookDataAsync(isbn);
        if (bookDto != null)
        {
            bookLocation = BookLocation.Server.GetDescription();
            _cacheService.Set(isbn, bookDto);
        }

        return bookDto;
    }

    public async Task<string[]> ProcessUniqueNumbersAsync(IFormFile file)
    {
        var uniqueIsbns = new HashSet<string>();

        using var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        string line;
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            var isbns = line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var isbn in isbns)
            {
                uniqueIsbns.Add(isbn);
            }
        }

        return uniqueIsbns.ToArray();
    }
}