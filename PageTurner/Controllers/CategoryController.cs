using Microsoft.AspNetCore.Mvc;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;

namespace PageTurner.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}
		public async Task<IActionResult> Index(int id,int pageNumber = 1,int pageSize = 10)
        {
			var isAdmin = User.IsInRole("Admin");
			var category = await _categoryRepository.GetCategoryAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			int TotalBooks = await _categoryRepository.GetTotalBooksCountAsync(id);

			List<BookCardViewModel> books = await _categoryRepository.GetBooksAsync(id, pageNumber, pageSize, isAdmin);

			var viewModel = new CategoryBooksViewModel
			{
				Books = books,
				CategoryName = category.Title,
				PagintionViewModel = new PagintionViewModel
				{
					CurrentPage = pageNumber,
					TotalPages = (int)Math.Ceiling((double)TotalBooks / pageSize)
				}
			};

			return View("Index",viewModel);
        }
    }
}
