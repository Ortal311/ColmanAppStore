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
            //Not sure! ( compares name instead of user.UserName) 
            string userName = User.Identity.Name;
            var cardUser = _context.PaymentMethod.Find(id).NameOnCard;
            if (!cardUser.Contains(userName) && !cardUser.Equals(userName))
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            var paymentMethod = await _context.PaymentMethod.FirstOrDefaultAsync(m => m.Id == id);
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name");

            return View();
        }

        // POST: PaymentMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Create([Bind("Id,NameOnCard,CardNumber,ExpiredDate,CVV,IdNumber")] PaymentMethod paymentMethod, int[] Users)
        {
            if (ModelState.IsValid)
            {
                paymentMethod.Users = new List<User>();
                var usr = _context.User.Include(u => u.PaymentMethods).Include(u => u.AppListUser);

                foreach (var item in usr)
                {
                    if (Users.Contains(item.Id)) //update user list depend on which users can use the payment method
                    {
                        paymentMethod.Users.Add(item);
                    }
                }

                _context.Add(paymentMethod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("HomePage", "Apps");
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

            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameOnCard,CardNumber,ExpiredDate,CVV,IdNumber")] PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> SearchPaymentMethod(int query)//search by name , category and description
        {

            var searchContext = _context.PaymentMethod.Where(a => a.IdNumber==query );

            return View("searchPaymentMethod", await searchContext.ToListAsync());
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

            var paymentMethod = await _context.PaymentMethod.FirstOrDefaultAsync(m => m.Id == id);
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
            var pm = _context.PaymentMethod.Include(u => u.Users);
            
            foreach(var p in pm)
            {
                if(p.Id==id)
                {
                    if(p.Users.Count() == 1) //connected to only one user
                    {
                        _context.PaymentMethod.Remove(paymentMethod);
                    }
                    else
                    {
                        String userName = User.Identity.Name;
                        var usr = _context.User.Include(p => p.PaymentMethods);
                        foreach(var us in usr)
                        {
                            if(us.Name.Equals(userName))
                            {
                                us.PaymentMethods.Remove(p); //remove the current payment method for the connected user
                                break;
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentMethodExists(int id)
        {
            return _context.PaymentMethod.Any(e => e.Id == id);
        }
    }
}
