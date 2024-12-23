using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Queries;

public static class BookQueryExtensions
{
    public static IQueryable<Book> WhereAuthorIs(this IQueryable<Book> books, Author author)
    {
        return books.Where(b => b.Author == author);
    }
    
    public static IQueryable<Book> WherePublicIdIs(this IQueryable<Book> query, Guid publicId)
    {
        return query.Where(l => l.PublicId == publicId);
    }

    public static IQueryable<Book> WhereTitleLike(this IQueryable<Book> query, string title)
    {
        return query
            .Where(b => EF.Functions.ILike(b.Title, $"%{title}%")
                || EF.Functions.ILike(b.Author.Name, $"%{title}%"));
    }
}