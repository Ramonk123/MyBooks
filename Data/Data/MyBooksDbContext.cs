using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class MyBooksDbContext : IdentityDbContext<User>
{
    public MyBooksDbContext(DbContextOptions<MyBooksDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryBook> LibraryBooks { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.HasOne(e => e.User)
                .WithMany(u => u.Libraries)
                .HasForeignKey(e => e.UserId);
        });
    }
}