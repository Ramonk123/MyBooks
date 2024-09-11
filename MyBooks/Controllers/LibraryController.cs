using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Libraries.Data;
using MyBooks.Libraries.Data.Enums;
using MyBooks.Libraries.Models;
using MyBooks.Libraries.Queries;
using MyBooks.Models.Overview;

namespace MyBooks.Controllers;

[Authorize]
public class LibraryController : Controller
{

    private readonly MyBooksDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;


    public LibraryController(
        MyBooksDbContext context,
        ILogger<AccountController> logger,
        UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpGet("Library/AddBookPopup/{libraryId}")]
    public IActionResult AddBookPopup(Guid libraryId)
    {
        return PartialView("~/Views/Shared/Popups/_AddBookPopup.cshtml", new AddBookPopupVM { LibraryId = libraryId });;
    }

    public async Task<Library> GetMyBooksLibrary()
    {
        var user = this.User;
        var userId = _userManager.GetUserId(user);
        
        if (userId == null)
        {
            throw new Exception("User not found");
        }

        var library = await _context.Libraries
            .Include(l => l.Books)
            .WhereUserIs(userId)
            .WhereTypeIs(LibraryType.DefaultLibrary)
            .SingleAsync();
        
        return library;
    }
}