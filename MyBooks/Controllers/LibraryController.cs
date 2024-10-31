using Data.Data;
using Data.Data.Enums;
using Data.Models;
using Data.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Config;
using MyBooks.Models.Library;
using MyBooks.Models.Overview;

namespace MyBooks.Controllers;

[Authorize]
public class LibraryController : Controller
{
    private readonly MyBooksDbContext _context;
    private readonly ILogger<LibraryController> _logger;
    private readonly UserManager<User> _userManager;


    public LibraryController(
        MyBooksDbContext context,
        ILogger<LibraryController> logger,
        UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null) return BadRequest();

        var defaultLibrary = await _context.Libraries
            .WhereUserIs(userId)
            .WhereTypeIs(LibraryType.DefaultLibrary)
            .Select(l => new LibraryVM
            {
                LibraryId = l.PublicId,
                Name = l.Name,
                Type = l.Type,
                Books = l.LibraryBooks.Select(lb => new LibraryBookVM
                {
                    BookId = lb.Book.PublicId,
                    AuthorId = lb.Book.Author.PublicId,
                    Title = lb.Book.Title,
                    Author = lb.Book.Author.Name,
                    Status = lb.Status.ToString(),
                    ThumbnailURL = lb.Book.ThumbnailURL
                }).ToList()
            })
            .SingleAsync();

        var libraries = await _context.Libraries.WhereUserIs(userId)
            .Select(l => new LibrarySidebarVM
            {
                Name = l.Name,
                PublicId = l.PublicId
            }).ToListAsync();

        return View("Index", new LibraryIndexVM
        {
            Library = defaultLibrary,
            Libraries = libraries
        });
    }

    public async Task<IActionResult> GetLibrary(Guid id)
    {
        var library = await _context.Libraries.WherePublicIdIs(id)
            .Select(l => new LibraryVM
            {
                LibraryId = l.PublicId,
                Name = l.Name,
                Type = l.Type,
                Books = l.LibraryBooks.Select(lb => new LibraryBookVM
                {
                    BookId = lb.Book.PublicId,
                    AuthorId = lb.Book.Author.PublicId,
                    Title = lb.Book.Title,
                    Author = lb.Book.Author.Name,
                    Status = lb.Status.ToString(),
                    ThumbnailURL = lb.Book.ThumbnailURL
                }).ToList()
            })
            .SingleAsync();

        return PartialView("Partials/_LibraryBooksPartial", library);
    }

    [HttpGet("Library/AddBookPopup/{libraryId}")]
    public IActionResult AddBookPopup(Guid libraryId)
    {
        return PartialView("~/Views/Shared/Popups/_AddBookPopup.cshtml", new AddBookPopupVM { LibraryId = libraryId });
        ;
    }

    public async Task<Library> GetMyBooksLibrary()
    {
        var user = User;
        var userId = _userManager.GetUserId(user);

        if (userId == null) throw new Exception("User not found");

        var library = await _context.Libraries
            .Include(l => l.Books)
            .WhereUserIs(userId)
            .WhereTypeIs(LibraryType.DefaultLibrary)
            .SingleAsync();

        return library;
    }

    [HttpGet]
    public async Task<Library> GetMyBooksLibrary(int id)
    {
        var user = User;
        var userId = _userManager.GetUserId(user);

        if (userId == null) throw new Exception("User not found");

        var library = await _context.Libraries
            .WhereIdIs(id)
            .Include(l => l.Books)
            .SingleAsync();

        return library;
    }

    [HttpGet(Routes.Library.Edit)]
    public async Task<IActionResult> EditLibrary([FromRoute] Guid id)
    {
        try
        {
            var library = await _context.Libraries.WherePublicIdIs(id).SingleAsync();

            return PartialView("Partials/_EditLibraryPartial", new EditLibraryVM
                {
                    LibraryId = library.PublicId
                }
            );
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Library not found" });
        }
    }

    [HttpPut(Routes.Library.Edit)]
    public async Task<IActionResult> EditLibrary([FromRoute] Guid id, [FromBody] string name)
    {
        var library = await _context.Libraries
            .WherePublicIdIs(id)
            .SingleOrDefaultAsync();

        if (library == null) return BadRequest();

        library.Name = name;
        _context.Libraries.Update(library);
        await _context.SaveChangesAsync();

        return Json(new { id, name });
    }

    [HttpGet(Routes.Library.Add)]
    public IActionResult OpenCreateLibraryModal()
    {
        return PartialView("Partials/_AddLibraryPartial");
    }

    [HttpDelete(Routes.Library.Delete)]
    public async Task<IActionResult> DeleteLibrary([FromRoute] Guid id)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return BadRequest("User not found");
        }
        
        var dbUser = await _context.Users.Where(u => u.Id == user.Id).SingleOrDefaultAsync();

        if (dbUser == null)
        {
            return BadRequest("User not found");
        }

        var library = await _context.Libraries
            .WherePublicIdIs(id)
            .SingleOrDefaultAsync();

        if (library == null)
        {
            return BadRequest();
        }

        if (library.UserId != dbUser.Id)
        {
            return BadRequest("User not allowed to delete this library");
        }

        _context.Libraries.Remove(library);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost(Routes.Library.Add)]
    public async Task<IActionResult> AddLibrary([FromBody] string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return BadRequest();

        var userLibraries = await _context.Libraries
            .WhereUserIs(user.Id)
            .ToListAsync();

        if (userLibraries.Any(l => string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { Message = "Library with this name already exists" });

        var library = new Library
        {
            PublicId = Guid.NewGuid(),
            Name = name,
            Type = LibraryType.Custom,
            UserId = user.Id
        };

        await _context.Libraries.AddAsync(library);
        await _context.SaveChangesAsync();

        return Json(new
        {
            id = library.PublicId,
            name
        });
    }
}