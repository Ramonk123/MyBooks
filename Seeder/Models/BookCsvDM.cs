namespace Data.Seeder.Models;

public class BookCsvDM
{
    public string Isbn13 { get; set; }        // Maps to the 'isbn13' column
    public string Isbn10 { get; set; }        // Maps to the 'isbn10' column
    public string Title { get; set; }         // Maps to the 'title' column
    public string Subtitle { get; set; }      // Maps to the 'subtitle' column (can be empty)
    public string Authors { get; set; }       // Maps to the 'authors' column (comma-separated authors)
    public string Categories { get; set; }    // Maps to the 'categories' column (comma-separated categories)
    public string Thumbnail { get; set; }     // Maps to the 'thumbnail' URL
    public string Description { get; set; }   // Maps to the 'description' column (can be quite large)
    public int? PublishedYear { get; set; }   
    public double? AverageRating { get; set; } 
    public int? NumPages { get; set; }         
    public int? RatingsCount { get; set; }     
    
}