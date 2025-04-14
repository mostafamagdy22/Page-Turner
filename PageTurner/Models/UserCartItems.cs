using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class UserCartItems
	{
		[ForeignKey("User")]
		public string UserID { get; set; }
		public ApplicationUser User { get; set; }
		[ForeignKey("Book")]
		public int BookID { get; set; }
		public Book Book { get; set; }
		public int Quantity { get; set; }
	}
}
