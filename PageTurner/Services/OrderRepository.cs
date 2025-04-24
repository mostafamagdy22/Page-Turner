using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Order> GetOrderByIdAsync(int id)
		{
			return await _context.Orders
				.Include(o => o.OrderDetails)
				.ThenInclude(od => od.Book)
				.Include(o => o.User)
				.FirstOrDefaultAsync(o => o.ID == id);
		}

		public async Task<IEnumerable<Order>> GetOrdersAsync()
		{
			return await _context.Orders
				.Include(o => o.OrderDetails)
				.Include(o => o.User)
				.ToListAsync();
		}

		public async Task UpdateOrderStatusAsync(Order order)
		{
			_context.Orders.Attach(order);
			_context.Entry(order).Property(o => o.Status).IsModified = true;
			await _context.SaveChangesAsync();
		}
	}
}
