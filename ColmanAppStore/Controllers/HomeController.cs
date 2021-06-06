using ColmanAppStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ColmanAppStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ColmanAppStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ColmanAppStoreContext _context;

        public HomeController(ILogger<HomeController> logger, ColmanAppStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public  IActionResult Index()
        {

            /*ViewData["top"] = _context.Apps.ToList();
             return View();*/

            /*var app =  _context.Apps.Include(a => a.Category).Include(l => l.Logo).Include(v => v.Videos).Include(i => i.Images).Include(r => r.Review).
               ThenInclude(u => u.UserName).FirstOrDefaultAsync(m => m.Id == id);
            if (app == null)
            {
                return NotFound();
            }

            return View(app);*/

            return View(_context.Apps.ToList());
        }

        [Authorize]
        public IActionResult Privacy()
        {

            /*  if(HttpContext.Session.GetString("email")==null)
              {
                  return RedirectToAction("Login", "Users");
              }*/
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult CategoryInfo() //(int id) ->to add the type of the category
        {
            Category category = new Category();
            category.Name = "Sport bkabka";

            return View(category);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
