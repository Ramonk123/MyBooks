using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Config;
using MyBooks.Libraries.Data;
using MyBooks.Libraries.Data.Enums;
using MyBooks.Libraries.Models;
using MyBooks.Libraries.Queries;
using MyBooks.Models.Overview;
using MyBooks.Services;
using PopularityService;

namespace MyBooks.Controllers;

[Authorize]
public class OverviewController : Controller
{
    private readonly OpenLibaryService _openLibraryService;
    private readonly MyBooksDbContext _context;
    private readonly PopularityQueryService _popularityService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<OverviewController> _logger;

    public OverviewController(
        OpenLibaryService openLibraryService,
        MyBooksDbContext context,
        PopularityQueryService popularityService,
        UserManager<User> userManager,
        ILogger<OverviewController> logger)
    {
        _openLibraryService = openLibraryService;
        _context = context;
        _popularityService = popularityService;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(model.Query))
        {
            return Ok();
        }

        var result = await _openLibraryService.Query(model.Query, model.SearchType);

        return Json(result);
    }

    public async Task<IActionResult> GetDefaultLibrary()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest();
        }

        var library = await _context.Libraries
            .WhereUserIs(user.Id)
            .WhereTypeIs(LibraryType.DefaultLibrary)
            .Select(l => new MyBooksVM
            {
                LibraryId = l.PublicId,
                LibraryName = l.Name,
                Books = l.LibraryBooks.Select(lb => new BookVM
                {
                    Name = lb.Book.Title,
                    Author = lb.Book.Author.Name,
                    PublicId = lb.Book.PublicId,
                    ISBN = lb.Book.Isbn,
                    ThumbnailUrl = lb.Book.ThumbnailURL,
                    DetailsUrl =
                        Url.Action("Details", "Books", new { id = lb.Book.PublicId }) // Ensure this is correctly set
                }).ToList() ?? new List<BookVM>()
            })
            .FirstOrDefaultAsync();

        if (library == null)
        {
            _logger.LogError($"User with id: {user.Id} does not have a default library");
            return BadRequest();
        }

        return PartialView("Partials/_MyBooksPartial", library);
    }

    [HttpGet(Routes.Book.Popular)]
    public IActionResult GetPopularBooks()
    {
        var popularBooks = _popularityService.GetPopularBooks().Select(pb => new PopularBooksVM
        {
            Author = pb.Author,
            AuthorId = pb.AuthorId,
            BookId = pb.BookId,
            PopularityScore = pb.PopularityScore,
            Title = pb.Title,
            ThumbnailUrl = pb.ThumbnailUrl // Ensure this property is included
        }).ToList();

        if (popularBooks == null || !popularBooks.Any())
        {
            return PartialView("Partials/_PopularBooksPartial", new List<PopularBooksVM>());
        }

        return PartialView("Partials/_PopularBooksPartial", popularBooks);
    }
}