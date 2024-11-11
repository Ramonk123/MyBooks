namespace MyBooks.Models.Book;

public class BookDetailVM
{
    public Guid PublicId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public int Rating { get; set; }

    public string? Library { get; set; }
    public List<ReviewVM>? Reviews { get; set; }
}

public class ReviewVM
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public string User { get; set; }
}