namespace MyBooks.Models.Book;

public class BookDetailVM
{
    public Guid PublicId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    
}