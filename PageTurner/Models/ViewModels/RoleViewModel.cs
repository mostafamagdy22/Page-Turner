using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class RoleViewModel
	{
		public string Id { get; set; }
		[Required]
		public string RoleName { get; set; }
	}
}
