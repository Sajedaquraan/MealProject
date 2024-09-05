using MealProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MealProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {

            int? id = null;

            if (HttpContext.Session.GetInt32("CustomerID") != null)
            {
                id = HttpContext.Session.GetInt32("CustomerID");
            }
            else if (HttpContext.Session.GetInt32("AdminID") != null)
            {
                id = HttpContext.Session.GetInt32("AdminID");
            }

            var user = _context.Customers.Where(x => x.Userid == id).SingleOrDefault();


            var defaultProfileImage = "default-profile-image.jpg";
            ViewBag.Name = user?.Username;
            ViewBag.Image = user?.Userimage ?? defaultProfileImage;
            ViewBag.Email = user?.Useremail;
            return View(user);
        }

        public IActionResult aboutus()
        {
            return View();
        }
        public IActionResult addrecipe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addrecipe([Bind("Userid,Recipesid,Title,Ingredients,Video,Image,ImageFile,CreatedAt,Userloginid")] Userrecipe userrecipe)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Userlogins
                    .Where(x => x.Userid == HttpContext.Session.GetInt32("CustomerID"))
                    .Select(x => x.Userloginid)
                    .SingleOrDefault();
                if (user != null)
                {
                    userrecipe.Userloginid = (int)user;
                    if (userrecipe.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + userrecipe.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userrecipe.ImageFile.CopyToAsync(fileStream);
                        }

                        userrecipe.Image = fileName;
                    }


                    _context.Add(userrecipe);
                    await _context.SaveChangesAsync();

                }
            }
            
            return View(userrecipe);
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
