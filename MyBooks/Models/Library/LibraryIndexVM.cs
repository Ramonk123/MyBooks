using Data.Data.Enums;

namespace MyBooks.Models.Library;


public class LibraryIndexVM
{
    public LibraryVM Library { get; set; }
    public List<LibrarySidebarVM> Libraries { get; set; }
}

public class LibrarySidebarVM
{
    
    public Guid PublicId { get; set; }
    public string Name { get; set; }
}

public class LibraryVM
{
    public Guid LibraryId { get; set; }
    public string Name { get; set; }
    public List<LibraryBookVM> Books { get; set; }
    public LibraryType Type { get; set; }
}

public class LibraryBookVM
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Status { get; set; }
    public string ThumbnailURL { get; set; }
}