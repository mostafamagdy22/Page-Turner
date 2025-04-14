using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManger;
		private readonly IProfileRepository _profileRepository;
		public ProfileController(UserManager<ApplicationUser> userManager,IProfileRepository profileRepository)
		{
			_userManger = userManager;
			_profileRepository = profileRepository;
		}
		public async Task<IActionResult> Index()
        {
			var userId = _userManger.GetUserId(User);
			var user = await _profileRepository.GetUserWithReviewsAsync(userId);
            ProfileViewModel viewModel = new ProfileViewModel();
			viewModel.UserName = user.UserName;
			viewModel.Email = user.Email;
			viewModel.Image = user.Image;
			viewModel.CreatedAt = user.CreatedAt;
			viewModel.Gender = user.Gender;
			viewModel.Reviews = user.Reviews.ToList();

			return View(viewModel);
        }
    }
}
