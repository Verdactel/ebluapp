using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eBluAppFinal.Data;
using eBluAppFinal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace eBluAppFinal.Controllers
{
    public class UsersController : Controller
    {
        private readonly eBluAppContext _context;

        public UsersController(eBluAppContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.Where(u => (u.Username == username))
                                        .Select(n => new User
                                        {
                                            Id = n.Id,
                                            Username = n.Username,
                                            Password = n.Password
                                        })
                                        .FirstOrDefault();
                var validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!validPassword || user == null) 
                {
                    ViewBag.LoginError = "Invalid Login";
                    return View();
                }
                else
                {
                    //Create the identity for the user  
                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, username)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Response.Cookies.Append("userID", user.Id.ToString(), new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = false,
                        IsEssential = true 
                    });
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Id,Email,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //check if email or username exists
                var req = _context.User.Where(u => (u.Username == user.Username) || (u.Email == user.Email));
                if(req.Count() > 0)
                {
                    ViewBag.AccExists = "That email or username is already in use.";
                    return View();
                }

                //hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHash;

                //add user
                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Users");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Username,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
