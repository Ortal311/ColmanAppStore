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
    public class AppImagesController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public AppImagesController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: AppImages
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.AppsImage.Include(a => a.App);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: AppImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appImage = await _context.AppsImage
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return NotFound();
            }

            return View(appImage);
        }

        // GET: AppImages/Create
        public IActionResult Create()
        {
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "DeveloperName");
            return View();
        }

        // POST: AppImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,AppId")] AppImage appImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "DeveloperName", appImage.AppId);
            return View(appImage);
        }

        // GET: AppImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appImage = await _context.AppsImage.FindAsync(id);
            if (appImage == null)
            {
                return NotFound();
            }
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "DeveloperName", appImage.AppId);
            return View(appImage);
        }

        // POST: AppImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,AppId")] AppImage appImage)
        {
            if (id != appImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppImageExists(appImage.Id))
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
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "DeveloperName", appImage.AppId);
            return View(appImage);
        }

        // GET: AppImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appImage = await _context.AppsImage
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return NotFound();
            }

            return View(appImage);
        }

        // POST: AppImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appImage = await _context.AppsImage.FindAsync(id);
            _context.AppsImage.Remove(appImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppImageExists(int id)
        {
            return _context.AppsImage.Any(e => e.Id == id);
        }
    }
}
