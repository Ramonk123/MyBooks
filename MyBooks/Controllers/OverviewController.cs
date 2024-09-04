using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBooks.Models.Overview;
using MyBooks.Services;

namespace MyBooks.Controllers;

[Authorize]
public class OverviewController : Controller
{

    private readonly OpenLibaryService _openLibraryService;

    public OverviewController(OpenLibaryService openLibraryService)
    {
        _openLibraryService = openLibraryService;
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
            return BadRequest(ModelState);  // This will return any model binding errors
        }

        if (string.IsNullOrEmpty(model.Query))
        {
            return Ok();
        }

        var result = await _openLibraryService.Query(model.Query, model.SearchType);

        return Json(result);
    }
    
    
}