using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class RolesRepository : IRolesRepository
	{
		private readonly ApplicationDbContext _context;
		public RolesRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<RoleViewModel>> GetRoles()
		{
			return await _context.Roles.Select(role => new RoleViewModel { Id = role.Id , RoleName = role.Name}).ToListAsync();
		}
	}
}
