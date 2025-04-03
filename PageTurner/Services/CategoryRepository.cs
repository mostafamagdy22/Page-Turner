using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _context;
		public CategoryRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.ToListAsync();
		}
	}
}
