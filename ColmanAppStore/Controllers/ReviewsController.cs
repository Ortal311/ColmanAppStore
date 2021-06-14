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
    public class ReviewsController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public ReviewsController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var colmanAppStoreContext = _context.Review.Include(r => r.App).Include(r => r.UserName);
            return View(await colmanAppStoreContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var review = await _context.Review
                 .Include(r => r.App)
                 .Include(r => r.UserName)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

      
        // GET: Reviews/Create
        public IActionResult Create(int id)
        {
            ViewData["AppId"] = id;

            foreach(var item in _context.Apps)
            {
                if(item.Id==id)
                {
                    ViewData["App"]= item;
                    break;
                }
            }

            ViewData["UserNameId"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,Raiting,PublishDate,AppId,UserNameId")] Review review, string userName)
        {
            if (ModelState.IsValid)
            {
                review.Id = 0;
                foreach (var item in _context.Apps)
                {
                    if (item.Id == review.AppId)
                    {
                        item.AverageRaiting = ((item.AverageRaiting * item.countReview) + review.Raiting) / (item.countReview + 1);
                        item.countReview++;

                        break;
                    }
                }
                foreach (var item in _context.User)
                {
                    if (userName.Equals(item.Name))
                    {
                        review.UserName = item;

                        break;
                    }
                }

                review.PublishDate = DateTime.Now;

                _context.Add(review);
                await _context.SaveChangesAsync();
                return Redirect("/Apps/Details/" + review.AppId);
            }
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", review.AppId);
            ViewData["UserNameId"] = new SelectList(_context.User, "Id", "Name", review.UserNameId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            /*string userName = User.Identity.Name;
            _context.Review.Find(id);
            string reviewWriter = _context.Review.Find(id).UserName.Name;
            if (userName != reviewWriter)
            {
                return Unauthorized();
                //return NotFound();

            }*/
            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", review.AppId);
            ViewData["UserNameId"] = new SelectList(_context.User, "Id", "Name", review.UserNameId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Raiting,PublishDate,AppId,UserNameId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }
            /*var rev = from r in _context.Review.Include(r => r.App).Include(r => r.UserName)
            string userName = User.Identity.Name;
            string reviewWriter = _context.Review.Find(id).UserName.Name;
            if (userName != reviewWriter)
            {
                return Unauthorized();
                //return NotFound();

            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    float sum = 0;
                    foreach (var item in _context.Review)
                    {
                        if (item.AppId == review.AppId)
                        {
                            sum += item.Raiting;
                        }
                    }
                    foreach (var item in _context.Apps) //updating the average raiting of the app
                    {
                        if (item.Id == review.AppId)
                        {
                            item.AverageRaiting = sum / item.countReview;
                            _context.Update(item);
                            break;
                        }
                    }

                    await _context.SaveChangesAsync(); //DB falls!!
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "Name", review.AppId);
            ViewData["UserNameId"] = new SelectList(_context.User, "Id", "Name", review.UserNameId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.App)
                .Include(r => r.UserName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            int reviewAppId = id;
            _context.Review.Remove(review);

            float sum = 0;
            foreach (var item in _context.Review)
            {
                if (item.AppId == reviewAppId)
                {
                    sum += item.Raiting;
                }
            }
            foreach (var item in _context.Apps) //updating the average raiting of the app
            {
                if (item.Id == reviewAppId)
                {
                    item.countReview--; //deleted one review of the app
                    item.AverageRaiting = sum / item.countReview;
                    _context.Update(item);
                    break;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UsersReview(int? id)
        {
            ViewModel model = new ViewModel();

            // Init:
            model.UserReviews = null;

            if (id == null)
            {
                return NotFound();
            }
            //REVIEW is correct!!!! but the view is not
            var review  = from r in _context.Review.Include(r => r.App).Include(r => r.UserName)
                         join usr in _context.User on r.UserNameId equals usr.Id
                         where id == r.UserNameId
                         select r ; //r.Title , r.Body , r.Raiting , r.PublishDate , r.App, r.UserName

            if (review == null)
            {
                return NotFound();
            }

            model.UserReviews = review.Distinct().Select(x => x).ToList(); ;// Using Select Many in order to flat from IEnumerable<IEnumerable<int>> to IEnumerable<int> and than to List<int>

            return View(model);
        }
    }
}
