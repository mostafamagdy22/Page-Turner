using Microsoft.AspNetCore.Identity;

namespace PageTurner.Models
{
	public class ApllicationUser : IdentityUser
	{
		public string Address { get; set; }
	}
}
