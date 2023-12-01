using System.Text;
using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;
using OpenLibrary.Domain;
using ClosedXML.Excel;

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

    public async Task<string> GetBooksInfoFromFileAsync(IFormFile file)
    {
        var tempDirectory = Path.GetTempPath();
        var filePath = Path.Combine(tempDirectory, "isbn_list.xlsx");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Books");
            worksheet.Cell("A1").Value = "Row Number";
            worksheet.Cell("B1").Value = "Data Retrieval Type";
            worksheet.Cell("C1").Value = "ISBN";
            worksheet.Cell("D1").Value = "Title";
            worksheet.Cell("E1").Value = "Subtitle";
            worksheet.Cell("F1").Value = "Author Name(s)";
            worksheet.Cell("G1").Value = "Number of Pages";
            worksheet.Cell("H1").Value = "Publish Date";

            var isbns = await ProcessUniqueNumbersAsync(file);
            int rowNumber = 2;

            foreach (var isbn in isbns)
            {
                var bookInfo = await FetchAndCacheBookDataAsync(isbn);
                if (bookInfo != null)
                {
                    worksheet.Cell(rowNumber, 1).Value = rowNumber - 1;
                    worksheet.Cell(rowNumber, 2).Value = bookLocation;
                    worksheet.Cell(rowNumber, 3).Value = isbn;
                    worksheet.Cell(rowNumber, 4).Value = bookInfo.Title;
                    worksheet.Cell(rowNumber, 5).Value = bookInfo.Subtitle;
                    worksheet.Cell(rowNumber, 6).Value = bookInfo.Authors;
                    worksheet.Cell(rowNumber, 7).Value = bookInfo.NumberOfPages;
                    worksheet.Cell(rowNumber, 8).Value = bookInfo.PublishDate;

                    rowNumber++;
                }
            }

            workbook.SaveAs(filePath);
        }

        return filePath;
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