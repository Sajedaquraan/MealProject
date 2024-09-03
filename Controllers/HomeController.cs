using MealProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MealProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult aboutus()
        {
            return View();
        }
        public IActionResult addrecipe()
        {
            return View();
        }
        public IActionResult BMI()
        {
            return View();
        }
        public IActionResult BMR()
        {
            return View();
        }
        public IActionResult filter()
        {
            return View();
        }
        public IActionResult MEALSUGGESTIONS()
        {
            return View();
        }
        public IActionResult NEUTRTION()
        {
            return View();
        }
        public IActionResult profile()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
