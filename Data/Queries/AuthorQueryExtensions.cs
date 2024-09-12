using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Queries;

public static class AuthorQueryExtensions
{
    public static IQueryable<Author> WhereAuthorNameLike(this IQueryable<Author> query, string name)
    {
        return query.Where(author => EF.Functions.Like(author.Name, $"%{name}%"));
    } 
}