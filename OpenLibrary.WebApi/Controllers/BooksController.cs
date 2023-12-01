using Microsoft.AspNetCore.Mvc;
using OpenLibrary.Application.Interfaces;

namespace OpenLibrary.WebApi.Controllers;

public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {

        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        var filePath = await _bookService.GetBooksInfoFromFileAsync(file);
        if (filePath == null)
            return NotFound();

        var memoryStream = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memoryStream);
        }
        memoryStream.Position = 0;

        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = "isbn_list.xlsx";

        return File(memoryStream, contentType, fileName);
    }
}

