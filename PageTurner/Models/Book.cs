using PageTurner.CustomValidtion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class Book
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string ISBN { get; set; }
		public string? Image { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public string Descreption { get; set; }
		
		public IEnumerable<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
	}
}
