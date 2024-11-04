using Data.Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBooks.Models.Account;

namespace MyBooks.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly MyBooksDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        MyBooksDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");
        
        var dbUser = await _context.Users.FindAsync(user.Id);
        if (dbUser == null) return RedirectToAction("Login");
        var model = new ProfileVM
        {
            Id = Guid.Parse(dbUser.Id),
            UserName = dbUser.UserName,
            Email = dbUser.Email,
            Name = dbUser.FullName,
        };
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM data)
    {
        if (!ModelState.IsValid) return View(data);

        var user = await _userManager.FindByEmailAsync(data.EmailAddress);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(data);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, data.Password, false, false);

        if (result.Succeeded)
        {
            HttpContext.Session.SetString("UserSession", "active");
            return RedirectToAction("Index", "Overview");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(data);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM data)
    {
        if (!ModelState.IsValid) return View(data);

        var user = new User
        {
            Email = data.EmailAddress,
            UserName = data.Username,
            FullName = data.FullName
        };

        var defaultLibraries = Library.CreateDefaultLibraries(user);


        var result = await _userManager.CreateAsync(user, data.Password);

        if (result.Succeeded)
        {
            _context.Libraries.AddRange(defaultLibraries);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Overview");
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);


        return View(data);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();

        return RedirectToAction("Login", "Account");
    }
}