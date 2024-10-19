using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskMe.Models;

namespace TaskMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var tasks = _context.Tasks.Where(t => t.UserId == userId.Value).ToList();
            return View(tasks);
        }

        [HttpPost]
        public IActionResult AddTask(Task task)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null && ModelState.IsValid)
            {
                task.UserId = userId.Value;
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult UpdateTask(Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
