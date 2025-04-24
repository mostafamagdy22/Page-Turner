using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PageTurner.Models;

namespace PageTurner.ViewComponents
{
	public class CategoriesMenuViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		public CategoriesMenuViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			try
			{
				var categories = await _context.Categories.ToListAsync();
				return View(categories);
			}
			catch (Exception ex)
			{
				return Content("Error loading categories.");
			}
		}
	}
}
