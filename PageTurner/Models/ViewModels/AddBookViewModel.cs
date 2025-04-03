using PageTurner.CustomValidtion;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class AddBookViewModel
	{
		public int ID { get; set; }
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
		public List<int> SelectedAuthors { get; set; } = new List<int>();
		public IEnumerable<SelectableAuthorViewModel> Authors { get; set; } = new List<SelectableAuthorViewModel>();
		[Required(ErrorMessage = "You must select at least one category!")]
		public List<int> SelectedCategories { get; set; } = new List<int>();
		public IEnumerable<Category> Categories { get; set; } = new List<Category>();
		public string? Image { get; set; }
		public IFormFile? ImageFile { get; set; }

		[Required(ErrorMessage = "You must select at least one publisher!")]
		public int SelectedPublisher { get; set; } 
		public IEnumerable<SelectablePublisherViewModel> Publishers { get; set; } = new List<SelectablePublisherViewModel>();

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Pages must be at least 1.")]
		public int Pages { get; set; }

		[Required]
		public string Formula { get; set; }
	}
}
