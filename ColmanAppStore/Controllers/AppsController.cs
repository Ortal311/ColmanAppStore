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
            var colmanAppStoreContext = _context.Apps.Include(a => a.Category).Include(l => l.Logo);
            return View(await colmanAppStoreContext.ToListAsync());
        }


        public async Task<IActionResult> Search(string query)//search by name , category and description ( price is not working)
        {

            /*(a.Price.CompareTo(float.Parse(query)) <= 0)*/
            /* a.Description.Contains(query)*/
            var searchContext = _context.Apps.Include(l => l.Logo).Include(c => c.Category)
               .Where(a => a.Name.Contains(query) || a.Category.Name.Contains(query) || (query == null));

            return View("Search", await searchContext.ToListAsync());
        }

        /* 
         public async Task<IActionResult> SearchByPrice(string query)
         {
             var searchContext = _context.Apps.Include(l => l.Logo).Include(c => c.Category).Where(a => (a.Price.CompareTo(float.Parse(query))<=0 );
             return View("Search", await searchContext.ToListAsync());
         }*/

        // GET: Apps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var app = await _context.Apps
                .Include(a => a.Category).Include(l => l.Logo).Include(v => v.Videos).Include(i => i.Images).Include(r => r.Review).
                ThenInclude(u => u.UserName).FirstOrDefaultAsync(m => m.Id == id);
            if (app == null)
            {
                return NotFound();
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["Images"] = new SelectList(_context.AppsImage, "Id", "Name");
            ViewData["Videos"] = new SelectList(_context.AppVideo, "Id", "Name");


            return View();
        }

        // POST: Apps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,publishDate,Logo,CategoryId,Size," +
                                                "AverageRaiting,countReview,DeveloperName,Images")] App app, int[] Images, int[] Videos)
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

                if (Images.Length > 0) //check if developer added more pics of the app
                {
                    app.Images = new List<AppImage>();
                    app.Images.AddRange(_context.AppsImage.Where(x => Images.Contains(x.Id)));

                    foreach (var item in app.Images)
                    {
                        item.AppId = app.Id;
                        item.App = app;
                    }
                }

                if (Videos.Length > 0) //check if developer added more videos of the app
                {
                    app.Videos = new List<AppVideo>();
                    app.Videos.AddRange(_context.AppVideo.Where(x => Videos.Contains(x.Id)));

                    foreach (var item in app.Videos)
                    {
                        item.AppId = app.Id;
                        item.App = app;
                    }
                }

                _context.Add(app);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", app.CategoryId);
            return View(app);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        // GET: Apps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string userName = User.Identity.Name;
            string appDevName = _context.Apps.Find(id).DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {

                return Unauthorized("No Access");
                //return NotFound();

            }

            if (id == null)
            {
                return NotFound();
            }

            var app = await _context.Apps.FindAsync(id);
            if (app == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", app.CategoryId);
            ViewData["Images"] = new SelectList(_context.AppsImage, "Id", "Name");
            ViewData["Videos"] = new SelectList(_context.AppVideo, "Id", "Name");
            return View(app);
        }

        // POST: Apps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,publishDate,CategoryId,Size,AverageRaiting,countReview,DeveloperName")] App app)
        {

            if (id != app.Id)
            {
                return NotFound();
            }
            string userName = User.Identity.Name;
            string appDevName = _context.Apps.Find(id).DeveloperName;
            Boolean isAdmin = User.IsInRole("Admin");
            if ((userName != appDevName) && !isAdmin)
            {
                return Unauthorized();
                //return NotFound();

            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(app);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppExists(app.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", app.CategoryId);
            return View(app);
        }

        // GET: Apps/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            string userName = User.Identity.Name;
            string appDevName = _context.Apps.Find(id).DeveloperName;
            if ((userName != appDevName))
            {

                return Unauthorized("No Access");
                //return NotFound();

            }
            if (id == null)
            {
                return NotFound();
            }

            var app = await _context.Apps
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (app == null)
            {
                return NotFound();
            }

            return View(app);
        }

        // POST: Apps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Programer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var app = await _context.Apps.FindAsync(id);
            _context.Apps.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppExists(int id)
        {
            return _context.Apps.Any(e => e.Id == id);
        }

        public async Task<IActionResult> HomePage()
        {
            var colmanAppStoreContext = _context.Apps.Include(a => a.Category).Include(l => l.Logo);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        public async Task<IActionResult> ReviewsGraph()
        {
            /*   float y=0;
               string x="";
               int len=_context.Apps.Count(); // total apps
               for(int i=0; i< len; i++)
               {
                  x= _context.Apps.ToList().ElementAt(i).Name; // X axis
                  y = _context.Apps.ToList().ElementAt(i).countReview; // Y axis

               }

               return View(x,y);*/
            return View();
        }
    }
}
