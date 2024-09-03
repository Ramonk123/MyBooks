namespace WebApplication1.Libraries.Models;

public class Book
{
     int Id { get; set; }
     Guid PublicId { get; set; }
     int Isbn { get; set; }
    
     string Title { get; set; }
     string Description { get; set; }
    
     int AuthorId { get; set; }
     public Author Author { get; set; }


}