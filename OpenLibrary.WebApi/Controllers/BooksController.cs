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
 
    public async Task<IActionResult> GetBookInfo(string isbn)
    {
        var book = await _bookService.GetBookInfoAsync(isbn);
        if (book == null)
            return NotFound();

        return Ok(book);
    }

}




