namespace MyBooks.Models.Shared;

public class AddBookToLibraryVM
{
    public BookVM Book { get; set; }
    public List<LibraryVM> Libraries { get; set; }
}

public class LibraryVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> BookIds { get; set; }
}

public class BookVM
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}