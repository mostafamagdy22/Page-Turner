using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class Review
	{
		public int ID { get; set; }
		[Required]
		public string ReviewContent { get; set; }
		[ForeignKey("user")]
		public string UserID { get; set; }
		public ApplicationUser user { get; set; }
		[ForeignKey("book")]
		public int BookID { get; set; }
		public Book book { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
