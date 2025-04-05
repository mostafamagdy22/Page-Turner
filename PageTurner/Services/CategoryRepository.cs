using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
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

		public async Task<Category> GetCategoryWithBooksAsync(int id)
		{
			return await _context.Categories.Include(c => c.BookCategories).ThenInclude(bc => bc.Book)
				.FirstOrDefaultAsync(category => category.ID == id);
		}
		public async Task<List<BookCardViewModel>> GetBooksAsync(int id, int CurrentPage, int PageSize,bool isAdmin)
		{
			var query = _context.Books
				.Where(b => b.BookCategories.Any(bc => bc.CategoryID == id))
					.Include(b => b.BookAuthors)
					.ThenInclude(ba => ba.Author)
					.Include(b => b.BookCategories)
					.ThenInclude(bc => bc.Category)
					.Include(b => b.Publisher)
				.Select(b => new BookCardViewModel
				{
					ID = b.ID,
					Title = b.Title,
					Image = string.IsNullOrEmpty(b.Image) ? "/images/Books/default.jpg" : b.Image,
					Price = b.Price,
					Publisher = b.Publisher,
					BookAuthors = b.BookAuthors.ToList(),
					BookCategories = b.BookCategories.ToList()
				}).AsQueryable();

			if (!isAdmin)
				query = query.AsNoTracking();

			return await query.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToListAsync();
		}

		public async Task<Category> GetCategoryAsync(int id)
		{
			return await _context.Categories.FirstOrDefaultAsync(c => c.ID == id);
		}

		public async Task<int> GetTotalBooksCountAsync(int id)
		{
			return await _context.Books
				.Where(b => b.BookCategories.Any(bc => bc.CategoryID == id))
					.CountAsync();
		}
	}
}
