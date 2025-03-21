using PageTurner.Models;

namespace PageTurner.Repositories.Interfaces
{
	public interface IBookRepository
	{
		public Task<IEnumerable<Book>> GetAllBooksAsync();
		public Task<IEnumerable<Book>> GetAllBooksAsyncWithAuthor();
		public Task<Book> GetByIsbnAsync(string isbn);
		public Task AddBookAsync(Book book);
		public Task<bool> UpdateBookAsync(Book book);
		public Task<bool> DeleteBook(string isbn);
		public Task<bool> IsIsbnExistAsync(string isbn); 
		public Task<Book> GetByIsbnAsyncWithAuthors(string isbn);
	}
}
