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
    public class AppVideosController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public AppVideosController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: AppVideos
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
                        return View(await _context.AppVideo.Include(a => a.App).ToListAsync());
                    }
                    else //programer user
                    {
                        var colmanAppStoreContext = _context.AppVideo.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(item.Name));
                        return View(await colmanAppStoreContext.ToListAsync());
                    }
                }
            }
            // in case couldn't find the user (can't happen when logged in)
            return View(await _context.AppVideo.ToListAsync());
        }

        // GET: AppVideos/Details/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var appVideo = await _context.AppVideo.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appVideo == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            String userName = User.Identity.Name;
            Boolean isAdmin = User.IsInRole("Admin");
            if (!appVideo.App.DeveloperName.Equals(userName) && !isAdmin)
                return RedirectToAction("AccessDenied", "Users");

            return View(appVideo);
        }

        // GET: AppVideos/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public IActionResult Create()
        {
            //ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");
            return View();
        }

        // POST: AppVideos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,Name,Video")] AppVideo appVideo)
        {
            if (ModelState.IsValid)
            {
                appVideo.AppId = 49; //default before change in app's create
                _context.Add(appVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name");
            return View(appVideo);
        }

        // GET: AppVideos/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var appVideo = await _context.AppVideo.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appVideo == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            string userName = User.Identity.Name;
            string appDevName = _context.AppVideo.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            if (User.IsInRole("Admin")) //admin user
            {
                ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", appVideo.AppId);
            }
            else //programer user
            {
                ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49).Where(x => x.DeveloperName.Equals(userName)), "Id", "Name", appVideo.AppId);
            }

            return View(appVideo);
        }

        // POST: AppVideos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Video,AppId")] AppVideo appVideo)
        {
            if (id != appVideo.Id || appVideo.AppId == 49)
            {
                return RedirectToAction("NotFound", "Home");
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
                        return RedirectToAction("NotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            string userName = User.Identity.Name;
            if (User.IsInRole("Admin")) //admin user
            {
                ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49), "Id", "Name", appVideo.AppId);
            }
            else //programer user
            {
                ViewData["AppId"] = new SelectList(_context.Apps.Where(a => a.Id != 49).Where(x => x.DeveloperName.Equals(userName)), "Id", "Name", appVideo.AppId);
            }

            return View(appVideo);
        }
        public async Task<IActionResult> SearchAppVideo(string query)//search by app name
        {
            string userName = User.Identity.Name;
            var searchContext = _context.AppVideo.Include(l => l.App).
                 Where(a => a.App.Name.Contains(query) || (query == null));
            if (!User.IsInRole("Admin")) // if developer- he will see only his apps ( admin sees everything)
            {
                searchContext = _context.AppVideo.Include(l => l.App).
                  Where(a => a.App.Name.Contains(query) || (query == null)).Where(u => u.App.DeveloperName.Equals(userName));
            }
            return View("SearchAppVideo", await searchContext.ToListAsync());
        }

        // GET: AppVideos/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var appVideo = await _context.AppVideo.Include(a => a.App).FirstOrDefaultAsync(m => m.Id == id);
            if (appVideo == null)
            {
                return NotFound();
            }
            string userName = User.Identity.Name;
            string appDevName = _context.AppVideo.Find(id).App.DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }
            return View(appVideo);
        }

        // POST: AppVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
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
