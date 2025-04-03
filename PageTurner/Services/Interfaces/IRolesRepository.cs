using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Interfaces
{
	public interface IRolesRepository
	{
		public Task<IEnumerable<RoleViewModel>> GetRoles();
	}
}
