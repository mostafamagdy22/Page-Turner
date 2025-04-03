using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IRolesRepository _rolesRepository;
		public RolesController(RoleManager<IdentityRole> roleManger,IRolesRepository rolesRepository)
		{
            _roleManger = roleManger;
            _rolesRepository = rolesRepository;
		}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<RoleViewModel> roles = await _rolesRepository.GetRoles();
            return View(roles);
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(RoleViewModel newRoleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = newRoleVM.RoleName;
				IdentityResult result = await _roleManger.CreateAsync(role);
                if (result.Succeeded)
                {
                    return View(new RoleViewModel());
                }
				foreach (IdentityError error in result.Errors)
				{
                    ModelState.AddModelError("", error.Description);
				}
			}
			return View(newRoleVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManger.FindByIdAsync(id);

			if (role == null)
            {
                return NotFound();
            }

            RoleViewModel roleModel = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(roleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
            {
                return BadRequest("Invalid Request!");
            }

            if (ModelState.IsValid)
            {
                IdentityRole role = await _roleManger.FindByIdAsync(id);

				if (role == null)
				{
					return NotFound();
				}

				role.Name = roleVM.RoleName;
                IdentityResult result = await _roleManger.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("",error.Description);
				}
			}

            return View(roleVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            IdentityRole role = await _roleManger.FindByNameAsync(name);

            if (role == null)
            {
                return NotFound();
            }

            IdentityResult result = await _roleManger.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return BadRequest();
        }
    }
}
