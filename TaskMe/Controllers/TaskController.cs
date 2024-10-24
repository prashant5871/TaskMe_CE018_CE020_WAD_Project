using Microsoft.AspNetCore.Mvc;
using TaskMe.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System;

namespace TaskMe.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult Index(DateTime? startDate, DateTime? endDate, int? priority)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Fetch all tasks for the user
            var tasks = _context.Tasks.Where(t => t.UserId == userId).AsQueryable();

            // Apply filters
            if (startDate.HasValue)
            {
                tasks = tasks.Where(t => t.CreatedAt <= startDate.Value);
            }

            if (endDate.HasValue)
            {
                tasks = tasks.Where(t => t.DueDate <= endDate.Value);
            }

            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == (TaskPriority)priority.Value);
            }

            // Separate tasks into remaining and completed
            var viewModel = new TaskViewModel
            {
                RemainingTasks = tasks.Where(t => !t.IsCompleted).ToList(),
                CompletedTasks = tasks.Where(t => t.IsCompleted).ToList()
            };

            return View(viewModel);
        }
    


    // GET: Add Task

    public IActionResult AddTask()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(new Task());
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
                return RedirectToAction("Index","Task");
            }
            return View("Login","Auth");
        }

        [HttpPost]
      
        public IActionResult UpdateTask(Task task)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {
                task.UserId = userId.Value;
                _context.Tasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction("Index", "Task");
            }
            return View("Index","Task");
        }

        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Task");
        }

        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var task = _context.Tasks.Find(id);
            if (task != null && task.UserId == userId.Value)
            {
                task.IsCompleted = true;
                _context.Tasks.Update(task);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Update Task
        public IActionResult UpdateTask(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

      
        
    }
}
