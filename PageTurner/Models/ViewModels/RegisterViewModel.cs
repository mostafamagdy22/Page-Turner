using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Username is required.")]
		[MinLength(2, ErrorMessage = "Username must be at least 2 characters.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Please confirm your password.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[MinLength(10, ErrorMessage = "Address must be at least 10 characters.")]
		public string Address { get; set; }

	}
}
