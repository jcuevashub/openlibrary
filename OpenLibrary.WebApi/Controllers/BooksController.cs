using System.Text;
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

    [HttpGet("{isbn}")]
    public async Task<IActionResult> GetBookInfo(string isbn)
    {
        var book = await _bookService.GetBookInfoAsync(isbn);
        if (book == null)
            return NotFound();

        return Ok();
    }

    [HttpPost("list")]
    public async Task<IActionResult> ProcessIsbns([FromBody] List<string> isbns)
    {
        var book = await _bookService.GetBooksInfoAsync("/Users/jacksoncuevas",isbns);
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost("file")]
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

