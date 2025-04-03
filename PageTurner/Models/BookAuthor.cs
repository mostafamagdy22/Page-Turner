using System.ComponentModel.DataAnnotations.Schema;

namespace PageTurner.Models
{
	public class BookAuthor
	{
		[ForeignKey(nameof(Book))]
		public int BookID { get; set; }
		public Book Book { get; set; }
		[ForeignKey(nameof(Author))]
		public int AuthorID { get; set; }
		public Author Author { get; set; }
	}
}
