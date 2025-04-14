using Microsoft.AspNetCore.Identity;
using PageTurner.Enums;

namespace PageTurner.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Address { get; set; }
		public Gender Gender { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string? Image { get; set; }
		public virtual ICollection<Review> Reviews { get; set; }
		public virtual Cart Cart { get; set; }
	}
}
