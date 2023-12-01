using System.Collections.Concurrent;
using Moq;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Application.Interfaces;

namespace OpenLibrary.UnitTest;

public static class MockCacheService
{

    private static readonly ConcurrentDictionary<string, BookDto> _cache = new ConcurrentDictionary<string, BookDto>();

    public static Mock<ICacheService<BookDto>> GetMockCacheService()
    {
        var mockBookService = new Mock<ICacheService<BookDto>>();

        mockBookService.Setup(service => service.Set(It.IsAny<string>(), It.IsAny<BookDto>()))
                .Callback((string key, BookDto book) =>
                {
                    _cache[key] = book;
                });


        mockBookService.Setup(service => service.Get(It.IsAny<string>()))
                 .Returns(new BookDto
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
