using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models
{
	public class Order
	{
		[Key]
		public int ID { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public string CustomerName { get; set; } = String.Empty;
		public decimal TotalAmount { get; set; }
		public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
	}
}
