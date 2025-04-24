using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Enums;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    [Authorize(Roles = "Admin")]
	public class OrdersController : Controller
    {
		private readonly IOrderRepository _orderRepository;
		public OrdersController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}
		public async Task<IActionResult> Details(int id)
        {
			Order order = await _orderRepository.GetOrderByIdAsync(id);
			List<OrderDetailsViewModel> details = new List<OrderDetailsViewModel>();

			details.AddRange(order.OrderDetails.Select(od => new OrderDetailsViewModel
			{
				Status = od.Order.Status,
				BookName = od.Book.Title,
				Quantity = od.Quantity,
				Price = od.Price,
				TotalAmount = od.Price * od.Quantity,
				Address = order.User.Address,
				PhoneNumber = order.User.PhoneNumber,
				Email = order.User.Email
			}));

			return View(details);
        }
		[HttpPost]
		public async Task<IActionResult> UpdateStatus(int orderID,OrderStatus newStatus)
		{
			Order order = await _orderRepository.GetOrderByIdAsync(orderID);
			if (order == null) return NotFound();

			order.Status = newStatus;
			await _orderRepository.UpdateOrderStatusAsync(order);

			return RedirectToAction("Details", new {id = orderID});
		}
    }
}