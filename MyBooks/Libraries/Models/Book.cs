namespace WebApplication1.Libraries.Models;

public class Book
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public int Isbn { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }
}