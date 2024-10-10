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

        var libraries = await _context.Libraries
            .WhereUserIs(userId)
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
            .ToListAsync();

        var defaultLibrary = libraries
            .SingleOrDefault(l => l.Type == LibraryType.DefaultLibrary);

        if (defaultLibrary == null) return BadRequest("Default library not found");


        return View("Index", new LibraryIndexVM
        {
            DefaultLibrary = defaultLibrary,
            Libraries = libraries
        });
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

        return Ok();
    }

    [HttpDelete(Routes.Library.Delete)]
    public async Task<IActionResult> DeleteLibrary([FromRoute] Guid id)
    {
        var library = await _context.Libraries
            .WherePublicIdIs(id)
            .SingleOrDefaultAsync();

        if (library == null) return BadRequest();

        _context.Libraries.Remove(library);

        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError($"Unable to delete library with id: {id} for user: {User.Identity.Name}");
            return BadRequest();
        }

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
        {
            return BadRequest(new {Message = "Library with this name already exists"});
        }
        
        var library = new Library
        {
            PublicId = Guid.NewGuid(),
            Name = name,
            Type = LibraryType.Custom,
            UserId = user.Id
        };

        await _context.Libraries.AddAsync(library);
        await _context.SaveChangesAsync();

        return Ok(name);
    }
}