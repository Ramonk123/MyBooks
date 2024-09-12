namespace Data.Seeder.Models;

public class BookDM
{
    public Guid PublicId { get; set; }
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ReleaseYear { get; set; }
    public double? AverageRating { get; set; } 
    public string? Description { get; set; }
    public string Author { get; set; }
    public string ThumbnailURL { get; set; }
}