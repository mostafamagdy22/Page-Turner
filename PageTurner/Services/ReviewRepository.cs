using Microsoft.EntityFrameworkCore;
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

		public async Task<Review> GetReviewByIdAsync(int reviewID)
		{
			return await _context.Reviews
				.Include(r => r.user)
				.Include(r => r.book)
				.FirstOrDefaultAsync(r => r.ID == reviewID);
		}

		public async Task<IEnumerable<Review>> GetReviewsByBookIdAsync(int bookID)
		{
			return await _context.Reviews
				.Where(r => r.BookID == bookID)
				.Include(r => r.user)
				.Include(r => r.book)
				.ToListAsync();
		}

		public async Task<bool> UpdateReviewAsync(Review review,string newContent)
		{
			try
			{
				if (review == null)
				{
					return false;
				}
				review.ReviewContent = newContent;

				_context.Attach(review);
				_context.Entry(review).Property(r => r.ReviewContent).IsModified = true;

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
