using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface IReviewRepository
	{
		public Task<bool> CreateReviewAsync(Review review);
		public Task<IEnumerable<Review>> GetReviewsByBookIdAsync(int bookID);
		public Task<Review> GetReviewByIdAsync(int reviewID);
		public Task<bool> UpdateReviewAsync(Review review, string newContent);
	}
}
