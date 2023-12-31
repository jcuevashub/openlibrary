﻿namespace OpenLibrary.Domain.Entities;

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public List<string> Authors { get; set; }
    public int? NumberOfPages { get; set; }
    public string PublishDate { get; set; }
}
