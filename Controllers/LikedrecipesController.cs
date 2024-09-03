using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealProject.Models;

namespace MealProject.Controllers
{
    public class LikedrecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public LikedrecipesController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment= environment;
        }

        // GET: Likedrecipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Likedrecipes.Include(l => l.Userlogin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Likedrecipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Likedrecipes == null)
            {
                return NotFound();
            }

            var likedrecipe = await _context.Likedrecipes
                .Include(l => l.Userlogin)
                .FirstOrDefaultAsync(m => m.Likedid == id);
            if (likedrecipe == null)
            {
                return NotFound();
            }

            return View(likedrecipe);
        }

        // GET: Likedrecipes/Create
        public IActionResult Create()
        {
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid");
            return View();
        }

        // POST: Likedrecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Likedid,Recipelabel,Recipedata,Recipeimage,Createdat,Userloginid,ImageFile")] Likedrecipe likedrecipe)
        {
            if (ModelState.IsValid)
            {
                if (likedrecipe.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()
                                      + "_"
                                      + likedrecipe.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await likedrecipe.ImageFile.CopyToAsync(fileStream);
                    }

                    likedrecipe.Recipeimage = fileName;
                }

                _context.Add(likedrecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", likedrecipe.Userloginid);
            return View(likedrecipe);
        }

        // GET: Likedrecipes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Likedrecipes == null)
            {
                return NotFound();
            }

            var likedrecipe = await _context.Likedrecipes.FindAsync(id);
            if (likedrecipe == null)
            {
                return NotFound();
            }
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", likedrecipe.Userloginid);
            return View(likedrecipe);
        }

        // POST: Likedrecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Likedid,Recipelabel,Recipedata,Recipeimage,Createdat,Userloginid,ImageFile")] Likedrecipe likedrecipe)
        {
            if (id != likedrecipe.Likedid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (likedrecipe.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString()
                                          + "_"
                                          + likedrecipe.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await likedrecipe.ImageFile.CopyToAsync(fileStream);
                        }

                        likedrecipe.Recipeimage = fileName;
                    }

                    _context.Update(likedrecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikedrecipeExists(likedrecipe.Likedid))
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
            ViewData["Userloginid"] = new SelectList(_context.Userlogins, "Userloginid", "Userloginid", likedrecipe.Userloginid);
            return View(likedrecipe);
        }

        // GET: Likedrecipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Likedrecipes == null)
            {
                return NotFound();
            }

            var likedrecipe = await _context.Likedrecipes
                .Include(l => l.Userlogin)
                .FirstOrDefaultAsync(m => m.Likedid == id);
            if (likedrecipe == null)
            {
                return NotFound();
            }

            return View(likedrecipe);
        }

        // POST: Likedrecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Likedrecipes == null)
            {
                return Problem("Entity set 'ModelContext.Likedrecipes'  is null.");
            }
            var likedrecipe = await _context.Likedrecipes.FindAsync(id);
            if (likedrecipe != null)
            {
                _context.Likedrecipes.Remove(likedrecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikedrecipeExists(decimal id)
        {
          return (_context.Likedrecipes?.Any(e => e.Likedid == id)).GetValueOrDefault();
        }
    }
}
