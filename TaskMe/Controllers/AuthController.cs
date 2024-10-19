// Controllers/AuthController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskMe.Models;

namespace TaskMe.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
