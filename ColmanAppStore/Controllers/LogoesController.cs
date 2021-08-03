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
    public class LogoesController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public LogoesController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: Logoes
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Index()
        {
            String userName = User.Identity.Name;
            foreach (var item in _context.User)
            {
                if (item.Name.Equals(userName))
                {
                    if (User.IsInRole("Admin")) //admin user
                    {
                        return View(await _context.Logo.Include(a => a.Apps).ToListAsync());
                    }
                    else //programer user
                    {
                        var colmanAppStoreContext = _context.Logo.Include(a => a.Apps).Where(x => x.Apps.DeveloperName.Equals(item.Name));
                        return View(await colmanAppStoreContext.ToListAsync());
                    }
                }
            }
            // in case couldn't find the user (can't happen when logged in)
            return View(await _context.Logo.ToListAsync());
        }

        // GET: Logoes/Details/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var logo = await _context.Logo.Include(l => l.Apps).FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(logo);
        }

        // GET: Logoes/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public IActionResult Create()
        {
            return RedirectToAction("NotFound", "Home");
            //ViewData["AppsId"] = new SelectList(_context.Apps, "Id", "Name");
            //return View();
        }

        // POST: Logoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,Image,AppsId")] Logo logo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppsId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", logo.AppsId);
            return View(logo);
        }

        // GET: Logoes/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var logo = await _context.Logo.Include(l => l.Apps).FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            string userName = User.Identity.Name;
            string appDevName = _context.Logo.Find(id).Apps.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if (!(userName.Equals(appDevName)) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            var apps = _context.Apps.Include(l => l.Logo).Where(a => a.Id != 49);
            foreach (var item in apps)
            {
                if (item.Logo.Id == id)
                {
                    ViewData["AppsId"] = item.Id;
                    ViewData["App"] = item;
                    break;
                }
            }
            return View(logo);
        }

        // POST: Logoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,AppsId")] Logo logo)
        {
            if (id != logo.Id)
            {
                return RedirectToAction("NotFound", "Home");
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
                        return RedirectToAction("NotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppsId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", logo.AppsId);
            return View(logo);
        }

        // GET: Logoes/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var logo = await _context.Logo.Include(l => l.Apps).FirstOrDefaultAsync(m => m.Id == id);
            if (logo == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            string userName = User.Identity.Name;
            string appDevName = _context.Logo.Find(id).Apps.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if (!(userName.Equals(appDevName)) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            return View(logo);
        }

        // POST: Logoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
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
