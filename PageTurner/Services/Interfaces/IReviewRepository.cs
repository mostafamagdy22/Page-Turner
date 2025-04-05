using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface IReviewRepository
	{
		public Task<bool> CreateReviewAsync(Review review);
	}
}
