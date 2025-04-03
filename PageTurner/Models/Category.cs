using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models
{
	public class Category
	{
		[Key]
		public int ID { get; set; }
		[Required]
		public string Title { get; set; }
		public virtual ICollection<BookCategories> BookCategories { get; set; } = new List<BookCategories>();
	}
}
