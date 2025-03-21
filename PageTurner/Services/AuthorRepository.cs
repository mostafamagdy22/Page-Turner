using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
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
	}
}
