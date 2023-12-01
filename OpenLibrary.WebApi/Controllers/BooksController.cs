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
    public async Task<IActionResult> UploadFile(string filePath, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        var book = await _bookService.GetBooksInfoFromFileAsync(filePath,file);
        if (book == null)
            return NotFound();
        return Ok(book);
    }
}

