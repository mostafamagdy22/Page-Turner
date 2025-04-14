using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class CartItem
	{
		public int ID { get; set; }
		[ForeignKey("Cart")]
		public int CartID { get; set; }
		public virtual Cart Cart { get; set; }
		[ForeignKey("Book")]
		public int BookID { get; set; }
		public int Quantity { get; set; }
		public virtual Book Book { get; set; }
	}
}