using PageTurner.Enums;

namespace PageTurner.Models.ViewModels
{
	public class ProfileViewModel
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string? Image { get; set; }
		public DateTime CreatedAt { get; set; }
		public Gender Gender { get; set; }
		public List<Review> Reviews { get; set; } = new List<Review>();
	}
}
