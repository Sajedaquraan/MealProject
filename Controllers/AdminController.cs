using MealProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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

            ViewBag.Userrecipe = _context.Userrecipes.Count();

            ViewBag.Likedrecipe = _context.Likedrecipes.Count();


            var Userlogins = _context.Userlogins.ToList();
            var Userrecipes = _context.Userrecipes.ToList();
            var Likedrecipes = _context.Likedrecipes.ToList();

            // Ensure that Userloginid matches in Userlogins and Likedrecipes
            var result = from ul in Userlogins
                         join ur in Userrecipes on ul.Userloginid equals ur.Userloginid
                         join lr in Likedrecipes on ul.Userloginid equals lr.Userloginid  // Match Userloginid in Likedrecipes
                         select new JoinTables
                         {
                             Userlogins = ul,
                             Userrecipes = ur,
                             Likedrecipes = lr  // Include liked recipes
                         };

            return View(result.ToList());  // Pass the result to the view
        }


    }
}
