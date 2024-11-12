using Data.Data;
using Data.Models;
using Data.Seeder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Seeder;

public static class BookSeeder
{
    public static async Task SeedBooksAsync(IServiceProvider serviceProvider, List<BookDM> books, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyBooksDbContext>();

        try
        {
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogError("Cannot connect to the database.");
                Environment.Exit(1);
            }

            var authors = await context.Authors.ToListAsync();
            var existingBooks = await context.Books.ToListAsync();
            var newBooks = new List<Book>();

            foreach (var book in books)
            {
                if (existingBooks.Any(b => b.Isbn == book.Isbn))
                {
                    logger.LogInformation("Book with ISBN {Isbn} already exists in the database.", book.Isbn);
                    continue;
                }
                try
                {
                    var formattedBook = new Book
                    {
                        PublicId = Guid.NewGuid(),
                        Title = book.Title,
                        Isbn = book.Isbn,
                        Subtitle = book.Subtitle,
                        ReleaseYear = book.ReleaseYear,
                        AverageRating = book.AverageRating,
                        Description = book.Description,
                        ThumbnailURL = book.ThumbnailURL
                    };

                    var author = authors.FirstOrDefault(a =>
                        a.Name.Equals(book.Author, StringComparison.OrdinalIgnoreCase));
                    if (author != null)
                    {
                        formattedBook.AuthorId = author.Id;
                        formattedBook.Author = author;
                    }
                    else
                    {
                        author = new Author
                        {
                            PublicId = Guid.NewGuid(),
                            Name = book.Author
                        };
                        formattedBook.Author = author;
                        newBooks.Add(formattedBook);
                        authors.Add(author);
                    }

                    logger.LogInformation("Processed book: {Title} by {Author}", book.Title, book.Author);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing book: {Title} by {Author}", book.Title,
                        book.Author);
                }
            }

            try
            {
                await context.Books.AddRangeAsync(newBooks);
                await context.SaveChangesAsync();
                logger.LogInformation("Successfully seeded books to the database.");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while saving books to the database.");
                Environment.Exit(1);
            }
        } catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while connecting to the database.");
            Environment.Exit(1);
        }
    }
}