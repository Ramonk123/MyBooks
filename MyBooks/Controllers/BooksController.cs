using System.Globalization;
using Data.Data;
using Data.Data.Enums;
using Data.Models;
using Data.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Config;
using MyBooks.Models.Book;
using MyBooks.Models.Shared;

namespace MyBooks.Controllers;

public class BooksController : Controller
{
    private readonly MyBooksDbContext _context;
    private readonly ILogger<BooksController> _logger;
    private readonly UserManager<User> _userManager;

    public BooksController(
        MyBooksDbContext context,
        UserManager<User> userManager,
        ILogger<BooksController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }


    public IActionResult Index([FromQuery] string? q)
    {
        ViewBag.Query = q;
        return View("Index");
    }

    [HttpGet(Routes.Book.Details)]
    public async Task<IActionResult> Details(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();

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

        if (book == null) return NotFound();

        return View("Details", book);
    }

    [HttpGet(Routes.Book.Search)]
    public async Task<List<SearchResultVM>> Search([FromRoute] string query)
    {
        var results = await _context.Books
            .WhereTitleLike(query)
            .Select(b => new SearchResultVM
            {
                Author = b.Author.Name,
                AuthorId = b.Author.PublicId,
                BookId = b.PublicId,
                ISBN = b.Isbn,
                Title = b.Title,
                Description = b.Description,
                ReleaseYear = b.ReleaseYear,
                AverageRating = b.AverageRating.GetValueOrDefault()
                    .ToString("0.0", CultureInfo.InvariantCulture),
                ThumbnailURL = b.ThumbnailURL
            })
            .ToListAsync();

        return results;
    }

    [HttpGet(Routes.Book.AddBookToLibrary)]
    public async Task<IActionResult> AddBookToLibrary(Guid bookId)
    {
        var userId = _userManager.GetUserId(User);
        
        var user = await _context.Users
            .Where(u => userId == u.Id)
            .FirstOrDefaultAsync();

        if (user == null) return BadRequest("User not found");

        var book = await _context.Books.WherePublicIdIs(bookId)
            .Select(b => new BookVM
            {
                Id = b.PublicId,
                Title = b.Title
            })
            .SingleAsync();

        var libraries = await _context.Libraries.WhereUserIs(user.Id)
            .Select(l => new LibraryVM
            {
                Id = l.PublicId,
                Name = l.Name,
                BookIds = l.LibraryBooks.Select(lb => lb.Book.PublicId).ToList()
            })
            .ToListAsync();

        return PartialView("Partials/_AddBookToLibraryPartial", new AddBookToLibraryVM
        {
            Book = book,
            Libraries = libraries,
        });
    }

    [HttpPost(Routes.Book.AddBookToLibrary)]
    public async Task<IActionResult> AddBookToLibrary([FromRoute]string bookId, [FromBody] AddBookToLibraryDM data)
    {
        if (!Guid.TryParse(bookId, out var parsedBookId))
        {
            return BadRequest("Invalid book id");
        }
        
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return BadRequest("User not found");

        var library = await _context.Libraries.WherePublicIdIs(data.LibraryId)
            .FirstOrDefaultAsync();

        if (library == null) return BadRequest("Library not found");

        if (library.UserId != user.Id) return BadRequest("Library does not belong to user");

        var book = await _context.Books.WherePublicIdIs(parsedBookId)
            .FirstOrDefaultAsync();

        if (book == null) return BadRequest("Book not found");

        var libraryBook = new LibraryBook
        {
            BookId = book.Id,
            LibraryId = library.Id,
            Status = BookStatus.Unread,
            UserId = user.Id

        };

        _context.LibraryBooks.Add(libraryBook);
        await _context.SaveChangesAsync();

        return Ok();
    }
}