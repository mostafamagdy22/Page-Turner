using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface IOrderRepository
	{
		public Task<IEnumerable<Order>> GetOrdersAsync();
		public Task<Order> GetOrderByIdAsync(int id);
		public Task UpdateOrderStatusAsync(Order order);
	}
}
