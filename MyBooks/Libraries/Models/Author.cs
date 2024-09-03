namespace WebApplication1.Libraries.Models;

public class Author
{
    private int Id { get; set; }
    private Guid PublicId { get; set; }
    
    private string Name { get; set; }

    private List<Book> Books { get; set; } = new List<Book>();
}