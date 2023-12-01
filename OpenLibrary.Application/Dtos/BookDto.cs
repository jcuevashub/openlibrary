namespace OpenLibrary.Application.Dtos;

public class BookDto
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Authors { get; set; }
    public int? NumberOfPages { get; set; }
    public string PublishDate { get; set; }
}
