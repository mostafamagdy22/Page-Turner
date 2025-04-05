using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Repositories.Interfaces
{
	public interface IAuthorRepository
	{
		public Task<IEnumerable<Author>> GetAuthorsAsync();
		public Task<bool> AddAuthorAsync(Author author);
		public Task<Author> GetAuthorByIDAsync(int id);
		public Task<bool> DeleteAuthorAsync(Author author);
		public Task<IEnumerable<int>> GetAuthorsIDsAsync();
		public Task<IEnumerable<string>> GetAuthorsNamesAsync();
		public Task<IEnumerable<SelectableAuthorViewModel>> selectableAuthorViewModelsAsync();
		public Task<(bool result,string image)> UpdateAuthorAsync(Author author);
		public Task<Author> GetAuthorByIDWithBooksAsync(int id);
	}
}
