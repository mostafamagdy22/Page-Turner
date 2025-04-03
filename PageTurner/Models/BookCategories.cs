using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class BookCategories
	{
		[ForeignKey(nameof(Book))]
		public int BookID { get; set; }
		public Book Book { get; set; }
		[ForeignKey(nameof(Category))]
		public int CategoryID { get; set; }
		public Category Category { get; set; }
	}
}
