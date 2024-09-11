using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBooks.Libraries.Models;

namespace MyBooks.Libraries.Data;

public class MyBooksDbContext : IdentityDbContext<User>
{
    
    
    public MyBooksDbContext(DbContextOptions<MyBooksDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Models.Library>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.HasOne(e => e.User)
                .WithMany(u => u.Libraries)
                .HasForeignKey(e => e.UserId);
        });
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryBook> LibraryBooks { get; set; }
    
}