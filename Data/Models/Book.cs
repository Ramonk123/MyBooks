namespace MyBooks.Libraries.Models;

public class Book
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string Isbn { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public string ThumbnailURL { get; set; }
    public ICollection<LibraryBook> LibraryBooks { get; set; }
}