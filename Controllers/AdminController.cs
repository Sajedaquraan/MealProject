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
            var id = HttpContext.Session.GetInt32("CustomerID");
            var users = _context.Customers.Where(x => x.Userid == id).SingleOrDefault();
            ViewBag.name = users.Username;
            ViewBag.image = users.ImageFile;
            ViewBag.email = users.Useremail;

            ViewBag.numberOfCustomer = _context.Customers.Count();

            
          
           
            return View();
        }
    }
}
