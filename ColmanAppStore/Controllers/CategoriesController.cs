using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanAppStore.Data;
using ColmanAppStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace ColmanAppStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public CategoriesController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.Where(a => a.Id != 9).ToListAsync());
        }

        // GET: Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 9)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _context.Category.Include(c => c.Apps).ThenInclude(c => c.Logo).FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            ViewData["Logo"] = new SelectList(_context.Logo, "Id", "Name");

            return View(category);
        }

        // GET: Categories/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 9)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id || id == 9)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return RedirectToAction("NotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> SearchByPrice(string query)
        {
            try
            {
                float.Parse(query);
                var searchContext = _context.Apps.Include(l => l.Logo).Where(a => (a.Price.CompareTo(float.Parse(query)) <= 0)).Where(a => a.Id != 49);
                return View("SearchByPrice", await searchContext.ToListAsync());
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Categories");
            }
        }


        // GET: Categories/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 9)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id != 9) //default category
            {
                var category = await _context.Category.Include(a => a.Apps).FirstOrDefaultAsync(m => m.Id == id);
                var categoryDefault = await _context.Category.FindAsync(9);
                foreach (var item in category.Apps) //default category (for not deleting all the apps in the category)
                {
                    item.Category = categoryDefault;
                    item.CategoryId = 9;
                    _context.Update(item);
                }
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
