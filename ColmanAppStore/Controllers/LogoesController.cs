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
    public class LogoesController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public LogoesController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: Logoes
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.Logo.Include(l => l.Apps);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: Logoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logo = await _context.Logo
                .Include(l => l.Apps)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null)
            {
                return NotFound();
            }

            return View(logo);
        }

        // GET: Logoes/Create
        public IActionResult Create()
        {
            ViewData["AppsId"] = new SelectList(_context.Apps, "Id", "DeveloperName");
            return View();
        }

        // POST: Logoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,AppsId")] Logo logo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppsId"] = new SelectList(_context.Apps, "Id", "DeveloperName", logo.AppsId);
            return View(logo);
        }

        // GET: Logoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logo = await _context.Logo.FindAsync(id);
            if (logo == null)
            {
                return NotFound();
            }
            ViewData["AppsId"] = new SelectList(_context.Apps, "Id", "DeveloperName", logo.AppsId);
            return View(logo);
        }

        // POST: Logoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,AppsId")] Logo logo)
        {
            if (id != logo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogoExists(logo.Id))
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
            ViewData["AppsId"] = new SelectList(_context.Apps, "Id", "DeveloperName", logo.AppsId);
            return View(logo);
        }

        // GET: Logoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logo = await _context.Logo
                .Include(l => l.Apps)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null)
            {
                return NotFound();
            }

            return View(logo);
        }

        // POST: Logoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logo = await _context.Logo.FindAsync(id);
            _context.Logo.Remove(logo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogoExists(int id)
        {
            return _context.Logo.Any(e => e.Id == id);
        }
    }
}
