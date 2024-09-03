using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBooks.Controllers;
using MyBooks.Libraries.Data;

namespace MyBooks.Libraries.Library;

[Authorize]
public class LibraryController : Controller
{

    private readonly MyBooksDbContext _context;
    private readonly ILogger<AccountController> _logger;

    public LibraryController(MyBooksDbContext context, ILogger<AccountController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("Index");
    }
}