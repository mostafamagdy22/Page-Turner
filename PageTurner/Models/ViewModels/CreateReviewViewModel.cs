using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class CreateReviewViewModel
	{
		[Required]
		public string ReviewContent { get; set; }
		[BindNever]
		public string UserID { get; set; }
		[Required]
		public int BookID { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
