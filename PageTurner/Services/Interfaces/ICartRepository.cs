using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Interfaces
{
	public interface ICartRepository
	{
		public List<CartItemViewModel> GetSessionCart();
		public Task SyncCartToDB(string userId);
		public Task<Cart> GetCartAsync(string userID);
		public Task<bool> UpdateCartItemQuantityAsync(string userID, int bookID, int quantity);
		public Task<bool> AddCartItemAsync(CartItem cartItem,Cart cart);
		public Task<bool> RemoveCartItemAsync(int cartItemID);
		public Task<bool> SyncCartToOrder(string userID);
	}
}
