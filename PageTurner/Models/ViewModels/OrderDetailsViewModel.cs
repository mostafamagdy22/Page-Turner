using PageTurner.Enums;

namespace PageTurner.Models.ViewModels
{
	public class OrderDetailsViewModel
	{
		public OrderStatus Status { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string BookName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
