using Microsoft.AspNetCore.Mvc;

namespace PageTurner.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
