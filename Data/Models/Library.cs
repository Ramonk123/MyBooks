using MyBooks.Libraries.Data.Enums;

namespace MyBooks.Libraries.Models;

public class Library
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public LibraryType Type { get; set; }
    public ICollection<LibraryBook> LibraryBooks { get; set; }
}