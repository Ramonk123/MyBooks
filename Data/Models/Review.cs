namespace Data.Models;

public class Review
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string Content { get; set; }
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTimeOffset.UtcNow.UtcDateTime;

    public int BookId { get; set; }
    public virtual Book Book { get; set; }

    public string UserId { get; set; }
    public virtual User User { get; set; }
}