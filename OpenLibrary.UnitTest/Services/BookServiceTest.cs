using System.Text;
using Microsoft.AspNetCore.Http;
using OpenLibrary.Application.Services;

namespace OpenLibrary.UnitTest;

public class BookServiceTest
{
    private readonly BookService _bookService;

    public BookServiceTest()
    {
        var mockCacheService = MockCacheService.GetMockCacheService();
        var mockOpenLibraryService = MockOpenLibraryService.GetMockOpenLibraryService();

        _bookService = new BookService(mockCacheService.Object, mockOpenLibraryService.Object);

    }

    [Fact]
    public async Task FetchBookDataAsync_Success()
    {
        const string content = "Hello, World!"; 
        const string fileName = "test.txt"; 
        var bytes = Encoding.UTF8.GetBytes(content);
        var memoryStream = new MemoryStream();
        memoryStream.Write(bytes, 0, bytes.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        var response = await _bookService.GetBooksInfoFromFileAsync("/",formFile);

    }

}
