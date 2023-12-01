using Moq;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;

namespace OpenLibrary.UnitTest;

public static class MockOpenLibraryService
{

    public static Mock<IOpenLibraryService> GetMockOpenLibraryService()
    {
        var mockBookService = new Mock<IOpenLibraryService>();

        mockBookService.Setup(service => service.FetchBookDataAsync(It.IsAny<string>()))
                 .ReturnsAsync(new BookDto
                 {
                     ISBN = "0201558025",
                     Title = "Cracking the Coding Interview",
                     Subtitle = "189 Programming Questions and Solutions",
                     Authors = "Gayle Laakmann McDowell",
                     NumberOfPages = 9,
                     PublishDate = "Nov 21, 2015"
                 });

        return mockBookService;
    }
}
