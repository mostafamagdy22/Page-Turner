using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Repositories.Interfaces;

namespace PageTurner.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly ApplicationDbContext _context;
		public BookRepository(ApplicationDbContext context) 
		{
			_context = context;
		}
		public async Task AddBookAsync(Book book)
		{
			await _context.Books.AddAsync(book);
			await _context.SaveChangesAsync();
		}
		public async Task<bool> IsIsbnExistAsync(string isbn)
		{
			return await _context.Books.AnyAsync(b => b.ISBN == isbn);
		}
		public async Task<bool> DeleteBook(string isbn)
		{
			Book book = await GetByIsbnAsync(isbn);
			if (book == null)
			{
				return false;
			}
			else
			{
				try
				{
					_context.Books.Remove(book);	
					await _context.SaveChangesAsync();
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		public async Task<IEnumerable<Book>> GetAllBooksAsync()
		{
			return await _context.Books.ToListAsync();
		}

		public async Task<IEnumerable<Book>> GetAllBooksAsyncWithAuthor()
		{
			return await _context.Books.Include(b => b.BookAuthors).ThenInclude(a  => a.Author).ToListAsync();
		}

		public async Task<Book> GetByIsbnAsync(string isbn)
		{
			return await _context.Books.FindAsync(isbn);
		}

		public Task<bool> UpdateBookAsync(Book book)
		{
			throw new NotImplementedException();
		}

		public async Task<Book> GetByIsbnAsyncWithAuthors(string isbn)
		{
			try
			{
				Book book = await _context.Books.Include(b => b.BookAuthors).ThenInclude(a => a.Author).FirstOrDefaultAsync(book => book.ISBN == isbn);
				if (book != null)
				{
					return book;
				}

				return null;	
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}
	}
}
