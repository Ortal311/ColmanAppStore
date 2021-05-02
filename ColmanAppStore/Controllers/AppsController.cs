using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanAppStore.Data;
using ColmanAppStore.Models;

namespace ColmanAppStore.Controllers
{
    public class AppsController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public AppsController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: Apps
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.Apps.Include(a => a.Category);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: Apps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apps == null)
            {
                return NotFound();
            }

            return View(apps);
        }

        // GET: Apps/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id");
            return View();
        }

        // POST: Apps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,publishDate,CategoryId,Size,Logo,Raiting,DeveloperName")] Apps apps)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", apps.CategoryId);
            return View(apps);
        }

        // GET: Apps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps.FindAsync(id);
            if (apps == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", apps.CategoryId);
            return View(apps);
        }

        // POST: Apps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,publishDate,CategoryId,Size,Logo,Raiting,DeveloperName")] Apps apps)
        {
            if (id != apps.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppsExists(apps.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", apps.CategoryId);
            return View(apps);
        }

        // GET: Apps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apps == null)
            {
                return NotFound();
            }

            return View(apps);
        }

        // POST: Apps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apps = await _context.Apps.FindAsync(id);
            _context.Apps.Remove(apps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppsExists(int id)
        {
            return _context.Apps.Any(e => e.Id == id);
        }
    }
}
