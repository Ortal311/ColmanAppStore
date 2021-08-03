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
            String userName = User.Identity.Name;
            foreach (var item in _context.User)
            {
                if (item.Name.Equals(userName))
                {
                    if (User.IsInRole("Admin")) //admin user
                    {
                        return View(await _context.AppsImage.Include(a => a.App).ToListAsync());
                    }
                    else //programer user
                    {
                        var colmanAppStoreContext = _context.AppsImage.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(item.Name));
                        return View(await colmanAppStoreContext.ToListAsync());
                    }
                }
            }
            // in case couldn't find the user (can't happen when logged in)
            return View(await _context.AppsImage.ToListAsync());
        }

        // GET: AppImages/Details/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var appImage = await _context.AppsImage.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            String userName = User.Identity.Name;
            Boolean isAdmin = User.IsInRole("Admin");
            if (!appImage.App.DeveloperName.Equals(userName) && !isAdmin)
                return RedirectToAction("AccessDenied", "Users");

            return View(appImage);
        }

        // GET: AppImages/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public IActionResult Create()
        {
            //ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");
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
                appImage.AppId = 49; //default app before change when app is created
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
                return RedirectToAction("NotFound", "Home");
            }
            var appImage = await _context.AppsImage.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            string userName = User.Identity.Name;
            string appDevName = _context.AppsImage.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((!userName.Equals(appDevName)) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", appImage.AppId);
            return View(appImage);
        }

        // POST: AppImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,AppId")] AppImage appImage)
        {
            if (id != appImage.Id || appImage.AppId == 49)
            {
                return RedirectToAction("NotFound", "Home");
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
                        return RedirectToAction("NotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", appImage.AppId);
            return View(appImage);
        }

        // GET: AppImages/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var appImage = await _context.AppsImage.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appImage == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            string userName = User.Identity.Name;
            string appDevName = _context.AppsImage.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((!userName.Equals(appDevName)) && !isAdmin)
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
