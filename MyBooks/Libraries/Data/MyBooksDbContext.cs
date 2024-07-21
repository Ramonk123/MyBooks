using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Libraries.Data;

public class MyBooksDbContext : DbContext
{
    public MyBooksDbContext(DbContextOptions<MyBooksDbContext> options) : base(options)
    {
        // A4Cj!0:[wpck48jmc15418cn2xcKAH8SD
    }
}