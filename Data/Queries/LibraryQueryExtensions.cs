using Data.Models;

namespace Data.Queries;

public static class LibraryQueryExtensions
{
    public static IQueryable<Library> WhereUserIs(this IQueryable<Models.Library> query, string userId)
    {
        return query.Where(l => l.UserId == userId);
    }
    
    public static IQueryable<Library> WhereTypeIs(this IQueryable<Models.Library> query, Data.Enums.LibraryType type)
    {
        return query.Where(l => l.Type == type);
    }
    
    public static IQueryable<Library> WherePublicIdIs(this IQueryable<Library> query, Guid publicId)
    {
        return query.Where(l => l.PublicId == publicId);
    }
    
    public static IQueryable<Library> WhereIdIs(this IQueryable<Library> query, int id)
    {
        return query.Where(l => l.Id == id);
    }
}