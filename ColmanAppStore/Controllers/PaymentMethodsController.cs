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
    public class PaymentMethodsController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public PaymentMethodsController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        // GET: PaymentMethods
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Index()
        {
            String userName = User.Identity.Name;
            var usr = _context.User.Include(u => u.PaymentMethods).Include(u => u.AppListUser);

            foreach (var item in usr)
            {
                if (item.Name.Equals(userName))
                {
                    var paymentmethods = _context.PaymentMethod.Where(i => i.Users.Contains(item));
                    return View(await paymentmethods.ToListAsync());
                }
            }
            return View(await _context.PaymentMethod.ToListAsync());
        }

        // GET: PaymentMethods/Details/5
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Details(int? id)
        {
      
            if (id == null)
            {
                return NotFound();
            }
            /*string userName = User.Identity.Name;
      string cardOwner = _context.PaymentMethod.Find(id).NameOnCard;
      if ((userName != cardOwner))
      {
          return Unauthorized("No Access");
          //return NotFound();

      }*/
            var paymentMethod = await _context.PaymentMethod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            return View(paymentMethod);
        }

        // GET: PaymentMethods/Create
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,NameOnCard,CardNumber,ExpiredDate,CVV,IdNumber")] PaymentMethod paymentMethod, string userName)
        {
            if (ModelState.IsValid)
            {
                //need to add which user?
                var usr = _context.User.Include(u => u.PaymentMethods).Include(u => u.AppListUser);
                foreach (var item in usr)
                {
                    if(userName.Equals(item.Name))
                    {
                        if (paymentMethod.Users == null)
                            paymentMethod.Users = new List<User>();
                        paymentMethod.Users.Add(item);
                        break;
                    }
                }

                _context.Add(paymentMethod);
                await _context.SaveChangesAsync();
               
            }
            return RedirectToAction("HomePage","Apps");
            //return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            /*string userName = User.Identity.Name;
      string cardOwner = _context.PaymentMethod.Find(id).NameOnCard;
      if ((userName != cardOwner))
      {
          return Unauthorized("No Access");
          //return NotFound();

      }*/

            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameOnCard,CardNumber,ExpiredDate,CVV,IdNumber")] PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return NotFound();
            }
            /*string userName = User.Identity.Name;
      string cardOwner = _context.PaymentMethod.Find(id).NameOnCard;
      if ((userName != cardOwner))
      {
          return Unauthorized("No Access");
          //return NotFound();

      }*/

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodExists(paymentMethod.Id))
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
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Delete/5
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           /* string userName = User.Identity.Name;
            string cardOwner = _context.PaymentMethod.
            if ((userName != cardOwner))
            {
            return Unauthorized("No Access");
            //return NotFound();

             }*/

            var paymentMethod = await _context.PaymentMethod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentMethodExists(int id)
        {
            return _context.PaymentMethod.Any(e => e.Id == id);
        }
    }
}
