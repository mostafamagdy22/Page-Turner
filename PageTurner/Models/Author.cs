using PageTurner.Enums;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models
{
	public class Author
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Descreption { get; set; }
		public string? Nationality { get; set; }
		public string? Image { get; set; }
		public bool IsDeleted { get; set; } = false;
		public Gender Gender { get; set; }
		public List<BookAuthor> BookAuthors { get; set; } = new();
	}
}
