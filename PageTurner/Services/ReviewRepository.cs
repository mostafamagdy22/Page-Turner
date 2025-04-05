using PageTurner.Models;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly ApplicationDbContext _context;
		public ReviewRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<bool> CreateReviewAsync(Review review)
		{
			try
			{
				await _context.Reviews.AddAsync(review);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				// Handle the exception (e.g., log it)
				return false;
			}
		}
	}
}
