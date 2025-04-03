using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Repositories.Interfaces;
using PageTurner.Services.Interfaces;

namespace PageTurner.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IFileService _fileService;
		public BookRepository(ApplicationDbContext context,IFileService fileService) 
		{
			_context = context;
			_fileService = fileService;
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
		public async Task<bool> DeleteBook(int id)
		{
			Book book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return false;
			}
			else
			{
				try
				{
					if (book.Image != null)
					{
						bool isImageDeleted = _fileService.Delete(book.Image,"images/Books");
					}
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
		public async Task<Book> GetBookAsync(int id)
		{
			return await _context.Books.Include(b => b.BookAuthors).Include(b => b.BookCategories).FirstOrDefaultAsync(b => b.ID == id);
		}
		public async Task<(bool Success, string OldImage)> UpdateBookAsync(Book book)
		{
			if (book == null)
				return (false, null);

			try
			{
				var existingBook = await _context.Books
				.Include(b => b.BookAuthors)
				.Include(b => b.BookCategories)
				.FirstOrDefaultAsync(b => b.ID == book.ID);

				if (existingBook == null)
					return (false, null);

				existingBook.BookAuthors = book.BookAuthors;
				existingBook.BookCategories = book.BookCategories;
				string oldImage = existingBook.Image;
				_context.Entry(existingBook).CurrentValues.SetValues(book);
				

				await _context.SaveChangesAsync();
				return (true, oldImage);
			}
			catch
			{
				return (false, null);
			}
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

		public async Task<IEnumerable<Book>> GetBooks(int pageNumber, int pageSize,bool isAdmin)
		{
			var query = _context.Books.Include(b => b.BookAuthors).ThenInclude(a => a.Author).Include(b => b.BookCategories).ThenInclude(bc => bc.Category).AsQueryable();
			if (!isAdmin)
				query = query.AsNoTracking();

			return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task<int> GetBooksCountAsync()
		{
			return await _context.Books.CountAsync();
		}

		public async Task<Book> GetDetailsByIDAsync(int id)
		{
			return await _context.Books.Include(b => b.BookAuthors)
				.ThenInclude(a => a.Author)
				.Include(b => b.BookCategories)
				.ThenInclude(bc => bc.Category)
				.Include(b => b.Publisher)
				.FirstOrDefaultAsync(b => b.ID == id);
		}
	}
}
