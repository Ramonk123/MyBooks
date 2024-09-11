using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBooks.Libraries.Data;
using MyBooks.Libraries.Data.Enums;
using MyBooks.Libraries.Models;
using MyBooks.Models.Account;
using WebApplication1.Models;

namespace MyBooks.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyBooksDbContext _context;
        private readonly ILogger<AccountController> _logger;


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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var user = await _userManager.FindByEmailAsync(data.EmailAddress);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(data);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, data.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
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
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var user = new User
            {
                Email = data.EmailAddress,
                UserName = data.Username,
                FullName = data.FullName,
            };
            
            var defaultLibrary = new Library
            {
                PublicId = Guid.NewGuid(),
                Name = "My Books",
                Type = LibraryType.DefaultLibrary,
                User = user,
                Books = new List<Book>()
            };

            
            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                _context.Libraries.Add(defaultLibrary);
                await _context.SaveChangesAsync();
                
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Overview");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            


            return View(data);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }
}
