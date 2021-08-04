using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColmanAppStore.Data;
using ColmanAppStore.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ColmanAppStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly ColmanAppStoreContext _context;

        public UsersController(ColmanAppStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,Email,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.Email == user.Email || u.Name == user.Name);
                if (q == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                    Signin(u);

                    return RedirectToAction("HomePage", "Apps");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply, cannot register this user.";
                }
            }

            return View(user);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Name, Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Password == user.Password && u.Email == user.Email
                        select u;

                if (q.Count() > 0)
                {
                    Signin(q.First());
                    return RedirectToAction("HomePage", "Apps");
                }
                else
                {
                    ViewData["Error"] = "Password and/or Email are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Role, account.UserType.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Users/Edit/5
        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            string connected = User.Identity.Name;
            string userName = _context.User.Find(id).Name;

            if (connected != userName)
            {
                return RedirectToAction("AccessDenied", "Users");
            }
            else
            {
                return View(user);
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,UserType")] User user)
        {
            if (id != user.Id)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("HomePage", "Apps");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return RedirectToAction("NotFound", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "Client,Admin,Programer")]
        public async Task<IActionResult> Account(string id) //get to user account info by name
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            else
            {
                var user = await _context.User.Include(x => x.PaymentMethods).Include(x => x.AppListUser).FirstOrDefaultAsync(m => m.Name == id);
                if (user == null)
                {
                    return RedirectToAction("NotFound", "Home");
                }
                string connected = User.Identity.Name;

                if (!connected.Equals(id))
                {
                    return RedirectToAction("AccessDenied", "Users");
                }

                foreach (var usr in _context.User.Include(a => a.AppListUser)) //calc list of user's payments
                {
                    if (usr.Name.Equals(connected))
                    {
                        ViewData["Payments"] = _context.Payment.Include(a => a.App).Include(c => c.PaymentMethod).Where(b => usr.AppListUser.Contains(b.App)).Where(u => usr.PaymentMethods.Contains(u.PaymentMethod));
                        break;
                    }
                }

                return View(user);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchUser(string query)//search by name
        {
            return Json(await _context.User.Where(a => a.Name.Contains(query)).ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client,Programer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            //only the user himself or admin can delete an account
            string connected = User.Identity.Name;
            string userName = _context.User.Find(id).Name;
            Boolean isAdmin = User.IsInRole("Admin");
            if (connected != userName && !isAdmin)
            {
                return RedirectToAction("AccessDenied", "Users");
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = _context.User.Include(a => a.AppListUser).Include(p => p.PaymentMethods);
            User u = null;
            foreach (var us in user)
            {
                if (us.Id == id)
                {
                    u = us;
                    if (us.AppListUser.Count() > 0) //the user downloaded at least 1 app
                    {
                        foreach (var item in _context.Payment)
                        {
                            if (item.Name.Contains(us.Name))
                            {
                                _context.Remove(item); //delete each payment the removed user did
                            }
                        }
                    }

                    if (us.PaymentMethods.Count() > 0)
                    {
                        var pm = _context.PaymentMethod.Include(u => u.Users);
                        foreach (var p in pm)
                        {
                            if (us.PaymentMethods.Contains(p))
                            {
                                if (p.Users.Count == 1) //the deleted user is the only owner of the payment method
                                {
                                    _context.Remove(p);
                                }
                            }
                        }
                    }
                    break;
                }
            }

            var rev = _context.Review.Include(a => a.UserName);
            foreach (var r in rev)
            {
                if (r.UserName == u)
                {
                    foreach (var a in _context.Apps)
                    {
                        if (a.Id == r.AppId)
                        { //update the avg raiting of the app without the review of the deleted user
                            a.AverageRaiting = (((a.AverageRaiting * a.countReview) - r.Raiting) / (a.countReview - 1));
                            a.countReview--;
                        }
                    }
                    _context.Remove(r);
                    break;
                }
            }

            _context.User.Remove(u);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _context.SaveChangesAsync();
            return RedirectToAction("HomePage", "Apps");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}