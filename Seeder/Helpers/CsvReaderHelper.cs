using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Data.Seeder.Models;

namespace Seeder.Helpers;

public class CsvReaderHelper
{
    public static List<BookDM> ReadBooksFromCsv(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true,
            HasHeaderRecord = true,
        });
        
        var records = csv.GetRecords<BookCsvDM>();
        var formattedBooks = records.Select(b => new BookDM
        {
            PublicId = Guid.NewGuid(),
            Isbn = b.Isbn10,
            Title = b.Title,
            Subtitle = b.Subtitle,
            ReleaseYear = b.PublishedYear.ToString(),
            Author = b.Authors.Split(',').FirstOrDefault()?.Trim(),
            AverageRating = b.AverageRating,
            Description = b.Description,
            ThumbnailURL = b.Thumbnail
        }).ToList();

        return formattedBooks;
    }
}