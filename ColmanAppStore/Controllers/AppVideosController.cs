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
    public class AppVideosController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public AppVideosController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: AppVideos
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.AppVideo.Include(a => a.App);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: AppVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVideo = await _context.AppVideo
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appVideo == null)
            {
                return NotFound();
            }

            return View(appVideo);
        }

        // GET: AppVideos/Create
        public IActionResult Create()
        {
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");
            return View();
        }

        // POST: AppVideos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Video")] AppVideo appVideo)
        {
            if (ModelState.IsValid)
            {
                appVideo.AppId = 1; //default before change in app's create
                _context.Add(appVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", appVideo.AppId);
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");

            return View(appVideo);
        }

        // GET: AppVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVideo = await _context.AppVideo.FindAsync(id);
            if (appVideo == null)
            {
                return NotFound();
            }
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", appVideo.AppId);
            return View(appVideo);
        }

        // POST: AppVideos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Video,AppId")] AppVideo appVideo)
        {
            if (id != appVideo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appVideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppVideoExists(appVideo.Id))
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
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "DeveloperName", appVideo.AppId);
            return View(appVideo);
        }

        // GET: AppVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVideo = await _context.AppVideo
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appVideo == null)
            {
                return NotFound();
            }

            return View(appVideo);
        }

        // POST: AppVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appVideo = await _context.AppVideo.FindAsync(id);
            _context.AppVideo.Remove(appVideo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppVideoExists(int id)
        {
            return _context.AppVideo.Any(e => e.Id == id);
        }
    }
}
