using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Repositories.Interfaces;

namespace PageTurner.Repositories
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly ApplicationDbContext _context;

		public AuthorRepository(ApplicationDbContext context) 
		{
			_context = context;
		}
		public async Task<IEnumerable<Author>> GetAuthorsAsync()
		{
			return await _context.Authors.ToListAsync();
		}
		public async Task<bool> AddAuthorAsync(Author author)
		{
			try
			{
				await _context.AddAsync(author);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<Author> GetAuthorByIDAsync(int id)
		{
			return await _context.Authors.FirstOrDefaultAsync(a => a.ID == id);
		}

		public async Task<bool> DeleteAuthorAsync(Author author)
		{
			if (author == null) return false;

			try
			{
				bool isAuthorLinkedWithBook = await _context.bookAuthors.AnyAsync(ba => ba.AuthorID == author.ID);
				if (isAuthorLinkedWithBook)
				{
					author.IsDeleted = true;
				}
				else
				{
					_context.Authors.Remove(author);
				}

				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<IEnumerable<int>> GetAuthorsIDsAsync()
		{
			return await _context.Authors.Select(a => a.ID).ToListAsync();
		}

		public async Task<IEnumerable<string>> GetAuthorsNamesAsync()
		{
			return await _context.Authors.Select(a => a.Name).ToListAsync();
		}
		public async Task<(bool result,string image)> UpdateAuthorAsync(Author author)
		{
			if (author == null)
				return (false, null);

			try
			{
				var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.ID == author.ID);

				if (existingAuthor == null)
					return (false, null);

				string oldImage = existingAuthor.Image;
				_context.Entry(existingAuthor).CurrentValues.SetValues(author);


				await _context.SaveChangesAsync();
				return (true, oldImage);
			}
			catch
			{
				return (false, null);
			}
		}
		public async Task<IEnumerable<SelectableAuthorViewModel>> selectableAuthorViewModelsAsync()
		{
			return await _context.Authors.Select(a => new SelectableAuthorViewModel
			{
				AuthorId = a.ID,
				AuthorName = a.Name
			}).ToListAsync();
		}
	}
}
