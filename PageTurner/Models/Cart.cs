using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class Cart
	{
		public int ID { get; set; }
		[ForeignKey("User")]
		public string UserID { get; set; }
		public ApplicationUser User { get; set; }
		public DateTime CreatedAt { get; set; }
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
	}
}
