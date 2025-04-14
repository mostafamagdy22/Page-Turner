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
				var cart = await _cartRepository.GetCartsAsync(userID);

				var cartViewModels = new List<CartItemViewModel>();

				foreach (var item in cart.CartItems)
				{
					Book book = await _bookRepository.GetBookAsync(item.BookID);
					if (book != null)
					{
						CartItemViewModel viewModel = new CartItemViewModel
						{
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
					return View(cart ?? new List<CartItemViewModel>());

				}
				return View(new List<CartItemViewModel>());
			}
        }
		public async Task<IActionResult> AddToCart(int bookID)
		{
			try
			{
				var cart = _cartRepository.GetSessionCart();
				var item = await _bookRepository.GetBookAsync(bookID);

				if (cart.Any(c => item.ID == c.BookID))
					cart.FirstOrDefault(c => c.BookID == item.ID).Quantity++;
				else
					cart.Add(new CartItemViewModel { BookID = bookID, Quantity = 1,Price = item.Price,BookTitle = item.Title });

				_httpContextAccessor.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

				return RedirectToAction("Index");
			}
			catch
			{
				// Handle the exception (e.g., log it)
				return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
			}
		}
	}
}
