using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class ProfileRepository : IProfileRepository
	{
		private readonly ApplicationDbContext _context;
		public ProfileRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<ApplicationUser> GetUserWithReviewsAsync(string userId)
		{
			return await _context.Users.Include(u => u.Reviews)
				.ThenInclude(r => r.book)
				.FirstOrDefaultAsync(u => u.Id == userId);
		}
	}
}
