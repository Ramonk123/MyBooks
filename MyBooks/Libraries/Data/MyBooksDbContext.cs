using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBooks.Libraries.Models;

namespace MyBooks.Libraries.Data;

public class MyBooksDbContext : IdentityDbContext<User>
{
    public MyBooksDbContext(DbContextOptions<MyBooksDbContext> options) : base(options)
    {
    }
    
}