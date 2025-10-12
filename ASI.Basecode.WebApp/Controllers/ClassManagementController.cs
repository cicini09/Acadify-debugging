using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ASI.Basecode.Data.Data;       // AppDbContext
using ASI.Basecode.Data.Models;     // User, Class, Course, etc.
using Student_Performance_Tracker.ViewModels.ClassManagement;

namespace Student_Performance_Tracker.Controllers
{
    [Authorize] // require login for all actions
    public class ClassManagementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ClassManagementController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // READ: All authenticated users can view the list
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .OrderBy(c => c.Course.CourseCode)
                .ToListAsync();

            return View(classes);
        }

        // CREATE (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateClassViewModel
            {
                Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync(),
                Teachers = await _userManager.Users.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
                model.Teachers = await _userManager.Users.ToListAsync();
                return View(model);
            }

            // verify course exists
            var course = await _context.Courses.FindAsync(model.CourseId);
            if (course == null)
            {
                ModelState.AddModelError(nameof(model.CourseId), "Selected course not found.");
                model.Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
                model.Teachers = await _userManager.Users.ToListAsync();
                return View(model);
            }

            // verify teacher exists
            var teacher = await _userManager.FindByIdAsync(model.TeacherId.ToString());
            if (teacher == null)
            {
                ModelState.AddModelError(nameof(model.TeacherId), "Selected teacher not found.");
                model.Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
                model.Teachers = await _userManager.Users.ToListAsync();
                return View(model);
            }

            // generate unique 8-char join code safely
            string joinCode;
            int tries = 0;
            do
            {
                joinCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                tries++;
                if (tries > 25)
                {
                    ModelState.AddModelError(string.Empty, "Unable to generate unique join code; try again.");
                    model.Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
                    model.Teachers = await _userManager.Users.ToListAsync();
                    return View(model);
                }
            } while (await _context.Classes.AnyAsync(c => c.JoinCode == joinCode));

            var cls = new Class
            {
                CourseId = model.CourseId,
                TeacherId = model.TeacherId,
                Semester = model.Semester,
                YearLevel = model.YearLevel,
                Schedule = model.Schedule,
                Room = model.Room,
                JoinCode = joinCode,
                JoinCodeGeneratedAt = DateTime.UtcNow,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Class created successfully.";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return RedirectToAction(nameof(Index));

            var vm = new EditClassViewModel
            {
                Id = cls.Id,
                CourseId = cls.CourseId,
                TeacherId = cls.TeacherId,
                Semester = cls.Semester,
                YearLevel = cls.YearLevel,
                Schedule = cls.Schedule,
                Room = cls.Room,
                IsActive = cls.IsActive,
                Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync(),
                Teachers = await _userManager.Users.ToListAsync(),
                JoinCode = cls.JoinCode
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
                model.Teachers = await _userManager.Users.ToListAsync();
                return View(model);
            }

            var cls = await _context.Classes.FindAsync(model.Id);
            if (cls == null)
            {
                TempData["Message"] = "Class not found.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            // update allowed fields (do not change JoinCode)
            cls.CourseId = model.CourseId;
            cls.TeacherId = model.TeacherId;
            cls.Semester = model.Semester;
            cls.YearLevel = model.YearLevel;
            cls.Schedule = model.Schedule;
            cls.Room = model.Room;
            cls.IsActive = model.IsActive;

            _context.Classes.Update(cls);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Class updated successfully.";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _context.Classes
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cls == null)
            {
                TempData["Message"] = "Class not found.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            if (cls.Enrollments != null && cls.Enrollments.Any())
            {
                TempData["Message"] = "Cannot delete class because students are enrolled. Remove enrollments first.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Class deleted successfully.";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
