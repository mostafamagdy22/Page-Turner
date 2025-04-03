using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Repositories.Interfaces;

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
		return RedirectToAction("Index","Books");
	}
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
