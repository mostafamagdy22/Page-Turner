using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Interfaces
{
	public interface ICartRepository
	{
		public List<CartItemViewModel> GetSessionCart();
		public Task SyncCartToDB(string userId);
		public Task<Cart> GetCartsAsync(string userID);
	}
}
