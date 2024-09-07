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

        [HttpPost]
        public async Task<IActionResult> SaveRecipe(string RecipeLabel, string RecipeData, string RecipeImage, decimal UserLoginId)
        {
            if (string.IsNullOrEmpty(RecipeLabel) || string.IsNullOrEmpty(RecipeData) || string.IsNullOrEmpty(RecipeImage))
            {
                return BadRequest("Invalid recipe data.");
            }

            // Check if the user is logged in by checking if the UserLoginId is in the session
            var userLoginId = HttpContext.Session.GetInt32("CustomerID");

            if (userLoginId == null)
            {
                // If the user is not logged in, redirect to the login page
                return RedirectToAction("Login", "RegisterAndLogin");
            }

            // Create a new Userrecipe object with the form data
            var Likedrecipe = new Likedrecipe
            {
                Recipelabel = RecipeLabel,
                Recipedata = RecipeData,
                Recipeimage = RecipeImage,
                Createdat = DateTime.Now,
                Userloginid = userLoginId
            };

            // Save the recipe to the database
            _context.Likedrecipes.Add(Likedrecipe);
            await _context.SaveChangesAsync();

            // Optionally redirect back to the page after saving the recipe
            return RedirectToAction("MEALSUGGESTIONS", "Home"); // Change "Home" to the appropriate controller
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
            var userLoginId = HttpContext.Session.GetInt32("CustomerID");

            var user =_context.Customers.Where(x=>x.Userid == userLoginId).SingleOrDefault();
            ViewBag.UserName = user.Username;
            ViewBag.UserImage = user.Userimage;
            ViewBag.UserEmail = user.Useremail;

            var Likedrecipe=_context.Likedrecipes.Where(x=>x.Userloginid== userLoginId).ToList();

            var Userrecipe = _context.Userrecipes.Where(x => x.Userloginid == userLoginId).ToList();

            var result = Tuple.Create<Customer,IEnumerable<Likedrecipe>,IEnumerable<Userrecipe>>(user, Likedrecipe, Userrecipe);
            return View(result);
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
