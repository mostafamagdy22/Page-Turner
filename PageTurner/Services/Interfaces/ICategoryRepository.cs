using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface ICategoryRepository
	{
		public Task<IEnumerable<Category>> GetCategoriesAsync();
	}
}
