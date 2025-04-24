using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Services.Interfaces;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DashBoardController : Controller
    {
		private readonly IOrderRepository _orderRepository;
		public DashBoardController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}
        public async Task<IActionResult> Index()
        {
			var orders = await _orderRepository.GetOrdersAsync();
			return View(orders);
        }
    }
}
