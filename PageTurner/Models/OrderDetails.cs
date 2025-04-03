using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models
{
	public class OrderDetails
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Order))]
		public int OrderId { get; set; }
		public Order Order { get; set; }

		[ForeignKey(nameof(Book))]
		public int BookID { get; set; }
		public Book Book { get; set; }

		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}