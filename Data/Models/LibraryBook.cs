using Data.Data.Enums;

namespace Data.Models;

public class LibraryBook
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public virtual User User { get; set; }
    
    public int LibraryId { get; set; }
    public virtual Library Library { get; set; }
    
    public int BookId { get; set; }
    public virtual Book Book { get; set; }
    
    public BookStatus Status { get; set; }
}