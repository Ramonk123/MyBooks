namespace Data.Models;

public class Publisher
{
    public string Name { get; set; }
    public Guid PublicId { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
}