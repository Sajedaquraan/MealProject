using MealProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public AdminController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
