using PageTurner.CustomValidtion;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class AddBookViewModel
	{
		[Required]
		[MinLength(5)]
		[UniqueISBN(ErrorMessage = "ISBN Is not Unique!")]
		public string ISBN { get; set; }
		[Required]
		[MinLength(2)]
		public string Title { get; set; }
		[Required]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }
		[Required]
		public string Descreption { get; set; }
		[Required(ErrorMessage = "You must add atleast author to this book!")]
		public List<int> AuthorIds { get; set; } = new List<int>(); 
		public IEnumerable<Author> Authors { get; set; } = new List<Author>();
		public IFormFile? ImageFile { get; set; }
	}
}
