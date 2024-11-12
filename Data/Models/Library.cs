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
            _createDefaultLibrary(user.Id),
            _createUnreadLibrary(user.Id),
            _createWishToReadLibrary(user.Id),
            _createReadLibrary(user.Id)
           ,
           
        };
    }

    public static Library _createDefaultLibrary(string userId)
    {
        return new Library
        {
            PublicId = Guid.NewGuid(),
            Name = "My Books",
            Type = LibraryType.DefaultLibrary,
            UserId = userId,
            Books = new List<Book>()
        };
    }
    public static Library _createUnreadLibrary(string userId)
    {
        return new Library
        {
            PublicId = Guid.NewGuid(),
            Name = "Unread",
            Type = LibraryType.Unread,
            UserId = userId,
            Books = new List<Book>()
        };
    }

    public static Library _createReadLibrary(string userId)
    {
        return new Library
        {
            PublicId = Guid.NewGuid(),
            Name = "Read",
            Type = LibraryType.Read,
            UserId = userId,
            Books = new List<Book>()
        };
    }

    public static Library _createWishToReadLibrary(string userId)
    {
        return new Library
        {
            PublicId = Guid.NewGuid(),
            Name = "Wish To Read",
            Type = LibraryType.WishToRead,
            UserId = userId,
            Books = new List<Book>()
        };
    }
}
