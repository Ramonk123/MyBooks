namespace MyBooks.Models.Book;

public class DeleteBookFromLibraryVM
{
    public Guid BookId { get; set; }
    public Guid LibraryId { get; set; }
}