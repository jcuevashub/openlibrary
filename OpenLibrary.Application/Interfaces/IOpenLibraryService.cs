using OpenLibrary.Application.Dtos;

namespace OpenLibrary.Application.Interfaces;

public interface IOpenLibraryService
{
    Task<BookDto> FetchBookDataAsync(string isbn);
    Task FetchBooksDataAsync(List<string> isbns);
    Task WriteToCsv(string outputFilePath, List<List<string>> allIsbnLists);
    List<List<string>> ReadInputFile(string filePath);
}
