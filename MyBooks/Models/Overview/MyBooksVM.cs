namespace MyBooks.Models.Overview;

public class MyBooksVM
{
    public Guid LibraryId { get; set; }
    public string LibraryName { get; set; }
    public List<BookVM> Books { get; set; } = new List<BookVM>();
}

public class BookVM
{
    public string Name { get; set; }
    public string Author { get; set; }
    public Guid PublicId { get; set; }
    public string ISBN { get; set; }
    public string ThumbnailUrl { get; set; }
    
    public string DetailsUrl { get; set; }
}