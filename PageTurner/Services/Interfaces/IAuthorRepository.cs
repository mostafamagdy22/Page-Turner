using PageTurner.Models;

namespace PageTurner.Repositories.Interfaces
{
	public interface IAuthorRepository
	{
		public Task<IEnumerable<Author>> GetAuthorsAsync();
		public Task<bool> AddAuthorAsync(Author author);
	}
}
