using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealProject.Models;
using Microsoft.Extensions.Hosting;

namespace MealProject.Controllers
{
    public class UserrecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserrecipesController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Userrecipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Userrecipes.Include(u => u.Userlogin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Userrecipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Userrecipes == null)
            {
                return NotFound();
            }

            var userrecipe = await _context.Userrecipes
                .Include(u => u.Userlogin)
                .FirstOrDefaultAsync(m => m.Recipesid == id);
            if (userrecipe == null)
            {
                return NotFound();
            }

            return View(userrecipe);
        }

        // GET: Userrecipes/Create
        public IActionResult Create()
        {
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid");
            return View();
        }

        // POST: Userrecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipesid,Title,Ingredients,Video,Image,CreatedAt,Userloginid,ImageFile")] Userrecipe userrecipe)
        {
            if (ModelState.IsValid)
            {
                if (userrecipe.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()
                                      + "_"
                                      + userrecipe.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await userrecipe.ImageFile.CopyToAsync(fileStream);
                    }

                    userrecipe.Image = fileName;
                }
                _context.Add(userrecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", userrecipe.Userloginid);
            return View(userrecipe);
        }

        // GET: Userrecipes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Userrecipes == null)
            {
                return NotFound();
            }

            var userrecipe = await _context.Userrecipes.FindAsync(id);
            if (userrecipe == null)
            {
                return NotFound();
            }
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", userrecipe.Userloginid);
            return View(userrecipe);
        }

        // POST: Userrecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Recipesid,Title,Ingredients,Video,Image,CreatedAt,Userloginid,ImageFile")] Userrecipe userrecipe)
        {
            if (id != userrecipe.Recipesid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userrecipe.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString()
                                          + "_"
                                          + userrecipe.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userrecipe.ImageFile.CopyToAsync(fileStream);
                        }

                        userrecipe.Image = fileName;
                    }

                    _context.Update(userrecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserrecipeExists(userrecipe.Recipesid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", userrecipe.Userloginid);
            return View(userrecipe);
        }

        // GET: Userrecipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Userrecipes == null)
            {
                return NotFound();
            }

            var userrecipe = await _context.Userrecipes
                .Include(u => u.Userlogin)
                .FirstOrDefaultAsync(m => m.Recipesid == id);
            if (userrecipe == null)
            {
                return NotFound();
            }

            return View(userrecipe);
        }

        // POST: Userrecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Userrecipes == null)
            {
                return Problem("Entity set 'ModelContext.Userrecipes'  is null.");
            }
            var userrecipe = await _context.Userrecipes.FindAsync(id);
            if (userrecipe != null)
            {
                _context.Userrecipes.Remove(userrecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserrecipeExists(decimal id)
        {
          return (_context.Userrecipes?.Any(e => e.Recipesid == id)).GetValueOrDefault();
        }
    }
}
