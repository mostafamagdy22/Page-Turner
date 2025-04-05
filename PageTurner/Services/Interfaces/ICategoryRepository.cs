using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Interfaces
{
	public interface ICategoryRepository
	{
		public Task<IEnumerable<Category>> GetCategoriesAsync();
		public Task<Category> GetCategoryWithBooksAsync(int id);
		public Task<List<BookCardViewModel>> GetBooksAsync(int id, int CurrentPage, int PageSize, bool isAdmin);
		public Task<Category> GetCategoryAsync(int id);
		public Task<int> GetTotalBooksCountAsync(int id);
	}
}
