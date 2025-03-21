using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Repositories.Interfaces;

namespace PageTurner.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
		public AuthorsController(IAuthorRepository authorRepository)
		{
			_authorRepository = authorRepository;
		}
		public async Task<IActionResult> Index()
        {
            IEnumerable<Author> authors = await _authorRepository.GetAuthorsAsync();
            return View(authors);
        }
        public async Task<IActionResult> Add()
        {

            return View();
        }
    }
}
