using Data.Data;
using Data.Models;
using Data.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBooks.Config;
using MyBooks.Models.Rating;

namespace MyBooks.Controllers;

public class ReviewController : Controller
{
    private readonly MyBooksDbContext _context;
    private readonly UserManager<User> _userManager;

    public ReviewController(MyBooksDbContext context,
        UserManager<User> userManager
    )
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost(Routes.Review.Create)]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDM data)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return BadRequest("User not found");

        var book = await _context.Books
            .WherePublicIdIs(data.BookId)
            .FirstOrDefaultAsync();

        if (book == null) return BadRequest("Book not found");

        var review = new Review
        {
            PublicId = Guid.NewGuid(),
            Content = data.Content,
            Rating = data.Rating,
            BookId = book.Id,
            UserId = user.Id
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return Ok();
    }
}