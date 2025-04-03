using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Repositories.Interfaces
{
	public interface IBookRepository
	{
		public Task<IEnumerable<Book>> GetAllBooksAsync();
		public Task<IEnumerable<Book>> GetAllBooksAsyncWithAuthor();
		public Task<Book> GetByIsbnAsync(string isbn);
		public Task<Book> GetDetailsByIDAsync(int id);
		public Task AddBookAsync(Book book);
		public Task<(bool Success, string OldImage)> UpdateBookAsync(Book book);
		public Task<bool> DeleteBook(int id);
		public Task<bool> IsIsbnExistAsync(string isbn); 
		public Task<Book> GetByIsbnAsyncWithAuthors(string isbn);
		public Task<IEnumerable<Book>> GetBooks(int pageNumber,int pageSize,bool isAdmin);
		public Task<Book> GetBookAsync(int id);
		public Task SaveChangesAsync();
		public Task<int> GetBooksCountAsync();
	}
}
