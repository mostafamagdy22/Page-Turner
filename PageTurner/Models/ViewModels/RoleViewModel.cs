using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PageTurner.Models.ViewModels
{
	public class RoleViewModel
	{
		[BindNever]
		public string Id { get; set; }
		[Required]
		public string RoleName { get; set; }
	}
}
