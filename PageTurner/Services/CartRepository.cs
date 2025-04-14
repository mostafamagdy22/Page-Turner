using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Text.Json;

namespace PageTurner.Services
{
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CartRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public List<CartItemViewModel> GetSessionCart()
		{
			var cartString = _httpContextAccessor.HttpContext.Session.GetString("Cart");

			if (string.IsNullOrEmpty(cartString))
			{
				var newCart = new List<CartItemViewModel>();
				_httpContextAccessor.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(newCart));
				return newCart;
			}
			else
			{
				var cart = JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);
				return cart ?? new List<CartItemViewModel>();
			}
		}

		public async Task SyncCartToDB(string userId)
		{
			var sessionCart = GetSessionCart();
			if (sessionCart == null || !sessionCart.Any())
			{
				return;
			}

			var cart = await _context.Carts.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userId);

			if (cart == null)
			{
				cart = new Cart
				{
					UserID = userId,
					CartItems = new List<CartItem>(),
					CreatedAt= DateTime.Now,
				};
				await _context.Carts.AddAsync(cart);
			}

			foreach(var item in sessionCart)
			{
				var existingItem = cart.CartItems.FirstOrDefault(ci => ci.BookID == item.BookID);
				if (existingItem != null)
					existingItem.Quantity++;
				else
					cart.CartItems.Add(new CartItem
					{
						BookID = item.BookID,
						Quantity = item.Quantity,
					});
			}
			await _context.SaveChangesAsync();
			_httpContextAccessor.HttpContext.Session.Remove("Cart");
		}
		public async Task<Cart> GetCartsAsync(string userID)
		{
			return await _context.Carts.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userID);
		}
	}
}
