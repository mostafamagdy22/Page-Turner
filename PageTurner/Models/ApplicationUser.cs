using Microsoft.AspNetCore.Identity;

namespace PageTurner.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Address { get; set; }
		public virtual ICollection<Review> Reviews { get; set; }
	}
}
