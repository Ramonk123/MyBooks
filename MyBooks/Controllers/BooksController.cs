using Data.Data;
using Data.Models;
using Data.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Config;
using MyBooks.Models.Book;
using PopularityService;

namespace MyBooks.Controllers;

public class BooksController : Controller
{
    private readonly MyBooksDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BooksController> _logger;
    
    public BooksController(
        MyBooksDbContext context,
        UserManager<User> userManager,
        ILogger<BooksController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost(Routes.Book.AddBook)]
    public async Task<IActionResult> AddBook([FromForm] AddBookVM model, Guid libraryId)
    {
        if (libraryId == Guid.Empty)
        {
            return BadRequest();
        }

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var library = await _context.Libraries.WherePublicIdIs(libraryId).SingleAsync();

        if (library.UserId != user.Id)
        {
            _logger.LogError($"Unauthorized user: ${user} tried to add book to library: ${library}");
            return BadRequest();
        }

        var book = new Book
        {
            PublicId = Guid.NewGuid(),
            Isbn = model.Isbn,
            Title = model.Title,
            Description = model.Description,
            ThumbnailURL = model.ThumbnailURL
        };

        var author = await _context.Authors.WhereAuthorNameLike(model.AuthorName).FirstOrDefaultAsync();

        if (author != null)
        {
            book.Author = author;
        }
        else
        {
            book.Author = new Author
            {
                PublicId = Guid.NewGuid(),
                Name = model.AuthorName,
                Books = new List<Book> { book }
            };
        }

        _context.Books.Add(book);
        _context.LibraryBooks.Add(new LibraryBook
        {
            Book = book,
            Library = library,
            User = user
        });

        await _context.SaveChangesAsync();

        return Ok(new { success = true });
    }

    [HttpGet(Routes.Book.Details)]
    public async Task<IActionResult> Details(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        var book = await _context.Books
            .WherePublicIdIs(id)
            .Select(b => new BookDetailVM
            {
                PublicId = b.PublicId,
                Title = b.Title,
                Author = b.Author.Name,
                ISBN = b.Isbn,
                Description = b.Description,
                ThumbnailUrl = b.ThumbnailURL
            })
            .FirstOrDefaultAsync();

        if (book == null)
        {
            return NotFound();
        }

        return View("Details", book);
    }
}