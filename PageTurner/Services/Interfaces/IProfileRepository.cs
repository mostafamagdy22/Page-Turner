using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface IProfileRepository
	{
		public Task<ApplicationUser> GetUserWithReviewsAsync(string userId);
	}
}
