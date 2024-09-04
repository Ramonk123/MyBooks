using WebApplication1.Libraries.Models;

namespace MyBooks.Libraries.Models;

public class Publisher
{
    public string Name { get; set; }
    public Guid PublicId { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
}