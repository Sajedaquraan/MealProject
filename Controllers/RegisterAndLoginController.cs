using Microsoft.AspNetCore.Mvc;

namespace MealProject.Controllers
{
    public class RegisterAndLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

    }
}
