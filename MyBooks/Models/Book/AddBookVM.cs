namespace MyBooks.Models.Book;

public class AddBookVM
{
    public int Id { get; set; }
    public string Isbn { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public string AuthorName { get; set; }
    public string ThumbnailURL { get; set; }
    
    public Guid LibraryId { get; set; }
}