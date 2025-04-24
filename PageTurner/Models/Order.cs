using PageTurner.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class Order
	{
		[Key]
		public int ID { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public string CustomerName { get; set; } = String.Empty;
		public decimal TotalAmount { get; set; }
		public OrderStatus Status { get; set; }
		[ForeignKey(nameof(User))]
		public string UserID { get; set; }
		public ApplicationUser User { get; set; }
		public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
	}
}
