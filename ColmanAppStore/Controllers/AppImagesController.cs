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
    public class AppImagesController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public AppImagesController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: AppImages
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.AppsImage.Include(a => a.App);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: AppImages/Details/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
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
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public IActionResult Create()
        {
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");
            return View();
        }

        // POST: AppImages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,Name,Image")] AppImage appImage)
        {
            if (ModelState.IsValid)
            {
                appImage.AppId = 1; //default before change in app's create
                _context.Add(appImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");

            return View(appImage);
        }

        // GET: AppImages/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appImage = await _context.AppsImage.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return NotFound();
            }
            string userName = User.Identity.Name;
            string appDevName = _context.AppsImage.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

          /*  var appImage = await _context.AppsImage.FindAsync(id);
            if (appImage == null)
            {
                return NotFound();
            }*/
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", appImage.AppId);
            return View(appImage);
        }

        // POST: AppImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,AppId")] AppImage appImage)
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
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", appImage.AppId);
            return View(appImage);
        }

        // GET: AppImages/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appImage = await _context.AppsImage.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return NotFound();
            }
            string userName = User.Identity.Name;
            string appDevName = _context.AppsImage.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            return View(appImage);
        }

        // POST: AppImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Programer")]
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
