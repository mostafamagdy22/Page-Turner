using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly IEmailSender _emailSender;
		private readonly ICartRepository _cartRepository;
		public AccountController(ICartRepository cartRepository,IEmailSender emailSender,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
		{
			_userManger = userManager;
            _signInManger = signInManager;
			_emailSender = emailSender;
			_cartRepository = cartRepository;
		}
        [AllowAnonymous]
        [HttpGet]
		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = model.UserName;
                userModel.Email = model.Email;
                userModel.Address = model.Address;
				userModel.PasswordHash = model.Password;
                IdentityResult result = await _userManger.CreateAsync(userModel,model.Password);

                if (result.Succeeded)
                {
                    // create cookie
                    await _signInManger.SignInAsync(userModel,false);
                    return RedirectToAction("Index","Books");
                }
                else
                {
					foreach (IdentityError error in result.Errors)
					{
                        ModelState.AddModelError("",error.Description);
					}
				}
            }
            catch (Exception ex)
            {
                View("Error",ex);
            }

			return View(model);
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email,PasswordHash = model.Password,UserName = model.UserName,Address = model.Address };
                IdentityResult result = await _userManger.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                   IdentityResult assignRoleResult = await _userManger.AddToRoleAsync(user,"Admin");
                   return RedirectToAction("Index","Roles");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser user = await _userManger.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    bool isAuth = await _userManger.CheckPasswordAsync(user, model.Password);

                    if (isAuth)
                    {
                        await _signInManger.SignInAsync(user, model.RememberMe);
                        await _cartRepository.SyncCartToDB(user.Id);
						return RedirectToAction("Index","Authors");
                    }
                }
                ModelState.AddModelError("", "Email and password invalid");
            }

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();
            return RedirectToAction("Register");
        }
        public IActionResult ForgetPassword()
        {
            var message = new Message(new string[] { "ttftf1782@gmail.com" }, "Test Email", "This is the content of the email");
            _emailSender.SendEmail(message);
			return Content("Email Sent");
		}
	}
}
