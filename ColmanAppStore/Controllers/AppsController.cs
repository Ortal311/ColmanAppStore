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
using Newtonsoft.Json;

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
            if (User.IsInRole("Admin"))
            {
                var colmanAppStoreContext = _context.Apps.Include(a => a.Category).Include(l => l.Logo).Where(a => a.Id != 49);
                return View(await colmanAppStoreContext.ToListAsync());
            }
            else
            {
                var colmanAppStoreContext = _context.Apps.Include(a => a.Category).Include(l => l.Logo).Where(a => a.Id != 49).Where(a => a.CategoryId != 9);
                return View(await colmanAppStoreContext.ToListAsync());
            }
        }


        //Search by name and category 
        public async Task<IActionResult> Search(string query)
        {
            var searchContext = _context.Apps.Include(l => l.Logo).Include(c => c.Category).Where(a => a.Id != 49)
          .Where(a => a.Name.Contains(query) || a.Category.Name.Contains(query) || a.DeveloperName.Equals(query) || (query == null));
            return View("Search", await searchContext.ToListAsync());
        }

        // GET: Apps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 49) //default app
            {
                return RedirectToAction("NotFound", "Home");
            }

            var app = await _context.Apps.Include(a => a.Category).Include(l => l.Logo).
                Include(v => v.Videos).Include(i => i.Images).Include(r => r.Review).
                ThenInclude(u => u.UserName).FirstOrDefaultAsync(m => m.Id == id);
            if (app == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            string userName = User.Identity.Name;
            var usr = _context.User.Include(u => u.PaymentMethods).Include(u => u.AppListUser);
            foreach (var item in usr)
            {
                if (item.Name.Equals(userName))
                {
                    ViewData["UserInfo"] = item;
                    ViewData["UserPMcount"] = item.PaymentMethods.Count();
                }
            }

            return View(app);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        // GET: Apps/Create
        public IActionResult Create()
        {
            String userName = User.Identity.Name;

            ViewData["CategoryId"] = new SelectList(_context.Category.Where(c => c.Id != 9), "Id", "Name");
            ViewData["Images"] = new SelectList(_context.AppsImage.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(userName) || x.AppId == 49), "Id", "Name");
            ViewData["Videos"] = new SelectList(_context.AppVideo.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(userName) || x.AppId == 49), "Id", "Name");

            return View();
        }

        // POST: Apps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,publishDate,Logo,CategoryId,Size," +
                                                "AverageRaiting,countReview,DeveloperName,Images,Videos")] App app, int[] Images, int[] Videos)
        {
            if (ModelState.IsValid)
            {
                Logo log = new Logo();
                log.Image = app.Logo.Image;
                log.Apps = app;
                log.AppsId = app.Id;
                _context.Add(log);

                app.publishDate = DateTime.Now;
                app.Logo = log;

                app.Images = new List<AppImage>();
                app.Images.AddRange(_context.AppsImage.Where(x => Images.Contains(x.Id)));
                foreach (var item in app.Images) //connect app images to the app
                {
                    item.AppId = app.Id;
                    item.App = app;
                }

                app.Videos = new List<AppVideo>();
                app.Videos.AddRange(_context.AppVideo.Where(x => Videos.Contains(x.Id)));
                foreach (var item in app.Videos)  //connect app video to the app
                {
                    item.AppId = app.Id;
                    item.App = app;
                }

                _context.Add(app);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            String userName = User.Identity.Name;

            ViewData["CategoryId"] = new SelectList(_context.Category.Where(c => c.Id != 9), "Id", "Name", app.CategoryId);
            ViewData["Images"] = new SelectList(_context.AppsImage.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(userName) || x.AppId == 49), "Id", "Name");
            ViewData["Videos"] = new SelectList(_context.AppVideo.Include(a => a.App).Where(x => x.App.DeveloperName.Equals(userName) || x.AppId == 49), "Id", "Name");

            return View(app);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        // GET: Apps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 49) //default app
            {
                return RedirectToAction("NotFound", "Home");
            }
            var app = await _context.Apps.FindAsync(id);
            if (app == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            string userName = User.Identity.Name;
            string appDevName = _context.Apps.Find(id).DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((!userName.Equals(appDevName)) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            ViewData["CategoryId"] = new SelectList(_context.Category.Where(c => c.Id != 9), "Id", "Name", app.CategoryId);

            Logo logo = null;
            var apps = _context.Apps.Include(x => x.Logo).Include(y => y.Images).Include(z => z.Videos);
            foreach (var item in apps)
            {
                if (item.Id == id)
                {
                    logo = item.Logo;
                    break;
                }
            }
            ViewData["Logo"] = logo;

            var AppReview = _context.Apps.Include(a => a.Review);
            foreach (var item in AppReview)
            {
                if (item.Id == id)
                {
                    ViewData["countReview"] = item.countReview;
                    ViewData["AverageRaiting"] = item.AverageRaiting;
                    break;
                }
            }

            var CurrentApp = await _context.Apps.Include(a => a.Images).Include(b => b.Videos).FirstOrDefaultAsync(m => m.Id == id);
            ViewData["Images"] = new SelectList(CurrentApp.Images, "Id", "Name");
            ViewData["Videos"] = new SelectList(CurrentApp.Videos, "Id", "Name");

            return View(app);
        }

        // POST: Apps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,publishDate,Logo,CategoryId,Size,AverageRaiting," +
                                            "countReview, DeveloperName,Images,Videos")] App app, int[] Images, int[] Videos)
        {
            if (id != app.Id || id == 49) //default app
            {
                return RedirectToAction("NotFound", "Home");
            }

            foreach (var item in _context.Logo) //get viewdata[Logo] (in case validation is false)
            {
                if (item.AppsId == app.Id)
                {
                    ViewData["Logo"] = item;
                    break;
                }
            }
            await _context.SaveChangesAsync();

            if (ModelState.ErrorCount == 2)
            {
                if (ModelState.ContainsKey("Images") && ModelState.ContainsKey("Videos"))
                    ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in _context.Logo) 
                    {
                        if (item.AppsId == app.Id)
                        {
                            item.Image = app.Logo.Image;
                            _context.Update(item);
                            app.Logo = item;
                            break;
                        }
                    }

                    app.publishDate = DateTime.Now;
                    _context.Update(app);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppExists(app.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", app.CategoryId);
            return View(app);
        }

        // GET: Apps/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 49) //default app
            {
                return RedirectToAction("NotFound", "Home");
            }

            string userName = User.Identity.Name;
            string appDevName = _context.Apps.Find(id).DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((!userName.Equals(appDevName)) && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            var app = await _context.Apps.Include(a => a.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (app == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(app);
        }

        // POST: Apps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id != 49)
            {
                var app = await _context.Apps.FindAsync(id);
                _context.Apps.Remove(app);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppExists(int id)
        {
            return _context.Apps.Any(e => e.Id == id);
        }

        public async Task<IActionResult> HomePage()
        {
            var colmanAppStoreContext = _context.Apps.Include(a => a.Category).Include(l => l.Logo).Where(a => a.Id != 49);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Graph()
        {
            var payedApps = _context.Payment.Include(a => a.App);
            Dictionary<String, int> map = new Dictionary<string, int>();

            foreach (var item in payedApps) //updating map of app(keys) and num of purchases(values)
            {
                if (map.ContainsKey(item.App.Name))
                {
                    map[item.App.Name]++;
                }
                else
                {
                    map.Add(item.App.Name, 1);
                }
            }

            var list = map.Keys.ToList();
            list.Sort();

            //Charts graph
            var query = from key in list select new { label = key, y = map[key] };
            ViewData["Graphs"] = JsonConvert.SerializeObject(query);

            //doughnut graph
            List<int> listY = new List<int>();
            listY.Add(map.Keys.Count); //count of downloaded apps
            listY.Add(_context.Apps.Count() - map.Keys.Count);

            List<String> listX = new List<string>();
            listX.Add("Downloaded apps");
            listX.Add("Not downloaded apps");

            ViewData["GraphUsageX"] = JsonConvert.SerializeObject(listX);
            ViewData["GraphUsageY"] = JsonConvert.SerializeObject(listY);

            return View();
        }
    }
}
