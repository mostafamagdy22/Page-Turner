using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Repositories.Interfaces;
using PageTurner.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    public class CartController : Controller
    {
		private readonly ICartRepository _cartRepository;
		private readonly IBookRepository _bookRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CartController(IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,IBookRepository bookRepository)
		{
			_cartRepository = cartRepository;
			_bookRepository = bookRepository;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<IActionResult> Index()
        {
			if (User.Identity.IsAuthenticated)
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var cart = await _cartRepository.GetCartAsync(userID);

				var cartViewModels = new List<CartItemViewModel>();

				foreach (var item in cart.CartItems)
				{
					Book book = await _bookRepository.GetBookAsync(item.BookID);
					if (book != null)
					{
						CartItemViewModel viewModel = new CartItemViewModel
						{
							ID = item.ID, // Assuming CartItem has an ID
							BookID = book.ID,
							BookTitle = book.Title,
							Quantity = item.Quantity,
							Price = book.Price
						};

						cartViewModels.Add(viewModel);
					}
				}
				return View(cartViewModels);
			}
			else
			{
				var cartString = _httpContextAccessor.HttpContext.Session.GetString("Cart");
				if (cartString != null)
				{
					var cart = JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);
					// Ensure to include CartItemID here if needed
					return View(cart ?? new List<CartItemViewModel>());
				}
				return View(new List<CartItemViewModel>());
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddToCart(int bookID)
		{
			try
			{
				var item = await _bookRepository.GetBookAsync(bookID);

				if (User.Identity.IsAuthenticated)
				{
					var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
					var dbCart = await _cartRepository.GetCartAsync(userID);

					var existingItem = dbCart.CartItems.FirstOrDefault(ci => ci.BookID == bookID);
					if (existingItem != null)
					{
						existingItem.Quantity++;
						await _cartRepository.UpdateCartItemQuantityAsync(userID, bookID, existingItem.Quantity);
					}
					else
					{
						var newItem = new CartItem
						{
							BookID = bookID,
							Quantity = 1,
							Book = item,
							CartID = dbCart.ID,
							Cart = dbCart
						};
						await _cartRepository.AddCartItemAsync(newItem, dbCart);
					}
				}
				else
				{
					var cart = _cartRepository.GetSessionCart();

					var existingItem = cart.FirstOrDefault(c => c.BookID == bookID);
					if (existingItem != null)
						existingItem.Quantity++;
					else
						cart.Add(new CartItemViewModel { BookID = bookID, Quantity = 1, Price = item.Price, BookTitle = item.Title });

					_httpContextAccessor.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
				}

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log it)
				return Json(new { success = false, message = ex.Message });
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequestViewModel request)
		{
			if (User.Identity.IsAuthenticated)
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				var dbCart = await _cartRepository.GetCartAsync(userID);
				var dbItem = dbCart.CartItems.FirstOrDefault(x => x.BookID == request.BookID);

				if (dbItem != null)
				{
					dbItem.Quantity = request.Quantity;
					await _cartRepository.UpdateCartItemQuantityAsync(userID, request.BookID, request.Quantity);
				}
				else
				{
					return BadRequest();
				}
			}
			else
			{
				string cartString = _httpContextAccessor.HttpContext.Session.GetString("Cart");
				if (cartString == null) return BadRequest();

				var cart = JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);
				var item = cart.FirstOrDefault(c => c.BookID == request.BookID);
				if (item == null) return NotFound();

				item.Quantity = request.Quantity;
				_httpContextAccessor.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
			}

			return Ok();
		}
		[HttpPost]
		public async Task<IActionResult> RemoveItem(int? cartItemID,int? bookID)
		{
			try
			{
				if (User.Identity.IsAuthenticated)
				{
					bool isRemoved = await _cartRepository.RemoveCartItemAsync(cartItemID.GetValueOrDefault());
					if (!isRemoved)
					{
						return Json(new { success = false, message = "Item not found." });
					}
					else
					{
						return Json(new { success = true });
					}
				}
				else
				{
					List<CartItemViewModel> cart = _cartRepository.GetSessionCart();
					var itemToRemove = cart.FirstOrDefault(c => c.BookID == bookID);

					if (itemToRemove != null)
					{
						cart.Remove(itemToRemove);
						_httpContextAccessor.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
						return Json(new { success = true });
					}
					else
					{
						return Json(new { success = false, message = "Item not found in session." });
					}
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "An error occurred." });
			}
		}
		[HttpGet]
		public async Task<IActionResult> GetCartCount()
		{
			int count = 0;
			if (User.Identity.IsAuthenticated)
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var dbCart = await _cartRepository.GetCartAsync(userID);
				count = dbCart?.CartItems.Sum(c => c.Quantity) ?? 0;
			}
			else
			{
				var cartString = _httpContextAccessor.HttpContext.Session.GetString("Cart");
				if (cartString != null)
				{
					var cart = JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);
					count = cart?.Sum(c => c.Quantity) ?? 0;
				}
			}

			return Json(new { Count = count });
		}
		[HttpGet]
		public async Task<IActionResult> Checkout()
		{
			if (User.Identity.IsAuthenticated)
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				await _cartRepository.SyncCartToOrder(userID);
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}
	}
}
