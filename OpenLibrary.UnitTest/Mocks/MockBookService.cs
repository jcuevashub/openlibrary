using Microsoft.AspNetCore.Http;
using Moq;
using OpenLibrary.Application.Interfaces;

namespace OpenLibrary.UnitTest;

public static class MockBookService
{

    public static Mock<IBookService> GetBookService()
    {
        var mockBookService = new Mock<IBookService>();

        mockBookService.Setup(service => service.GetBooksInfoFromFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
                 .ReturnsAsync("");
        return mockBookService;
    }
}
