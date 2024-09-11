namespace MyBooks.Models.Overview;

public class PopularBooksVM
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Guid AuthorId { get; set; }
    public int PopularityScore { get; set; }
    public string ThumbnailUrl { get; set; }
}