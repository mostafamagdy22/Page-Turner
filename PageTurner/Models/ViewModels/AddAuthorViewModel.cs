using PageTurner.Enums;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class AddAuthorViewModel
	{
		public int ID { get; set; }
		[Required]
		[MinLength(2)]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }
		public string Descreption { get; set; }
		[Required]
		public string Nationality { get; set; }
		public string? Image { get; set; }
		public IFormFile? ImageFile { get; set; }
		[Required]
		public Gender Gender { get; set; }
	}
}
