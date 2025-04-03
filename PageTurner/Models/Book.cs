using PageTurner.CustomValidtion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class Book
	{
		[Key]
		public int ID { get; set; }
		public string ISBN { get; set; }
		public string? Image { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public string Descreption { get; set; }

		[ForeignKey("Publisher")]
		public int PublisherID { get; set; }
		public Publisher Publisher { get; set; }

		public int Pages { get; set; }
		public string Formula { get; set; }

		public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
		public virtual List<BookCategories> BookCategories { get; set; } = new List<BookCategories>();
		public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
	}
}
