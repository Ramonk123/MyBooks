namespace MyBooks.Models.Rating;

public class CreateReviewDM
{
    public Guid BookId { get; set; }
    public string Content { get; set; }
    public double Rating { get; set; }
}