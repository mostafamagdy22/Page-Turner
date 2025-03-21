using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Repositories.Interfaces;
using static System.Reflection.Metadata.BlobBuilder;

namespace PageTurner.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _bookRepository;
    public HomeController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
	}
	public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllBooksAsyncWithAuthor();
		return View("~/Views/Books/Index.cshtml", books);
	}
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
