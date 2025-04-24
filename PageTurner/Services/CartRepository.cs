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
		public async Task<Cart> GetCartAsync(string userID)
		{
			return await _context.Carts.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userID);
		}

		public async Task<bool> UpdateCartItemQuantityAsync(string userID, int bookID, int quantity)
		{
			if (quantity == 0)
			{
				var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.BookID == bookID && ci.Cart.UserID == userID);
				if (item != null)
				{
					_context.CartItems.Remove(item);
					await _context.SaveChangesAsync();
					return true;
				}
			}
			var cart = await _context.Carts.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userID);

			if (cart != null)
			{
				var item = cart.CartItems.FirstOrDefault(ci => ci.BookID == bookID);
				if (item != null)
				{
					item.Quantity = quantity;
					_context.CartItems.Update(item);
					await _context.SaveChangesAsync();
					return true;
				}
			}

			return false;
		}

		public async Task<bool> AddCartItemAsync(CartItem cartItem,Cart cart)
		{

			if (cart == null)
				return false;

			cart.CartItems.Add(cartItem);
			
			await _context.SaveChangesAsync();
			
			return true;
		}

		public async Task<bool> RemoveCartItemAsync(int cartItemID)
		{
			CartItem item = await _context.CartItems.FindAsync(cartItemID);

			if (item != null)
			{
				_context.CartItems.Remove(item);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<bool> SyncCartToOrder(string userID)
		{
			var user = await _context.Users
				.Include(u => u.Cart)
				.ThenInclude(c => c.CartItems)
				.ThenInclude(ci => ci.Book)
				.FirstOrDefaultAsync(u => u.Id == userID);

			if (user == null || user.Cart == null || !user.Cart.CartItems.Any())
			{
				return false;
			}
			else
			{
				var orderDetails = user.Cart.CartItems.Select(item => new OrderDetails
				{
					BookID = item.BookID,
					Quantity = item.Quantity,
					Book = item.Book,
					Price = item.Book.Price,
				}).ToList();

				Order order = new Order
				{
					UserID = userID,
					OrderDate = DateTime.Now,
					OrderDetails = orderDetails,
					CustomerName = user.UserName,
					TotalAmount = orderDetails.Sum(od => od.Price * od.Quantity)
				};

				await _context.Orders.AddAsync(order);
				_context.CartItems.RemoveRange(user.Cart.CartItems);
				await _context.SaveChangesAsync();
			}

			return true;
		}
	}
}
