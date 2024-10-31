using Data.Data.Enums;

namespace Data.Models;

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

    public static List<Library> CreateDefaultLibraries(User user)
    {
        return new List<Library>()
        {
            new Library
            {
                PublicId = Guid.NewGuid(),
                Name = "My Books",
                Type = LibraryType.DefaultLibrary,
                User = user,
                Books = new List<Book>()
            },
            new Library
            {
                PublicId = Guid.NewGuid(),
                Name = "Wish To Read",
                Type = LibraryType.WishToRead,
                User = user,
                Books = new List<Book>()
            },
            new Library
            {
                PublicId = Guid.NewGuid(),
                Name = "Read",
                Type = LibraryType.Read,
                User = user,
                Books = new List<Book>()
            }
        };
    }
}
