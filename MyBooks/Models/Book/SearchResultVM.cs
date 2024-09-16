namespace MyBooks.Models.Book;

public class SearchResultVM
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string ISBN { get; set; }
    public string? ReleaseYear { get; set; }
    public double? Rating { get; set; }
    public string ThumbnailURL { get; set; }
}