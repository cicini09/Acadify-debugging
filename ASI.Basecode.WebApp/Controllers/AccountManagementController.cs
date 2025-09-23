using ASI.Basecode.Data.Data;
using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Performance_Tracker.ViewModels.AccountManagement;

namespace ASI.Basecode.Web.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AccountManagementController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // READ: View all users from database
        public async Task<IActionResult> Index()
        {
            try
            {
                // Gets data from Users table with related enrollments and classes
                var users = await _userManager.Users
                    .Include(u => u.Enrollments)
                        .ThenInclude(e => e.Class)
                    .Include(u => u.ClassesTeaching)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                ViewBag.Message = TempData["Message"];
                ViewBag.MessageType = TempData["MessageType"];

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error loading users: " + ex.Message;
                ViewBag.MessageType = "error";
                return View(new List<User>());
            }
        }

        // READ: View individual user details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Gets detailed data from Users table with navigation properties
                var user = await _userManager.Users
                    .Include(u => u.Enrollments)
                        .ThenInclude(e => e.Class)
                            .ThenInclude(c => c.Course)
                    .Include(u => u.ClassesTeaching)
                        .ThenInclude(c => c.Course)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    TempData["Message"] = "User not found.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error loading user details: " + ex.Message;
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

        // CREATE: Show create form
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        // CREATE: Add new users to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if username or email already exists
                    var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);
                    var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);

                    if (existingUserByUsername != null)
                    {
                        ModelState.AddModelError("UserName", "Username already exists.");
                        return View(model);
                    }

                    if (existingUserByEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    // Create new user - Auto-generates Id, CreatedAt
                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsApproved = model.IsApproved,
                        EmailConfirmed = true, // Auto-confirm admin created accounts
                        CreatedAt = DateTime.UtcNow // Auto-generates CreatedAt
                    };

                    // Create password (handled by Identity)
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        TempData["Message"] = $"User '{user.FirstName} {user.LastName}' created successfully!";
                        TempData["MessageType"] = "success";
                        return RedirectToAction("Index");
                    }

                    // Add Identity errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error creating user: " + ex.Message);
                }
            }
            return View(model);
        }

        // UPDATE: Show edit form (Admin can only edit Username and IsApproved)
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    TempData["Message"] = "User not found.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!, // Read-only display
                    FirstName = user.FirstName, // Read-only display
                    LastName = user.LastName, // Read-only display
                    IsApproved = user.IsApproved
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error loading user: " + ex.Message;
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

        // UPDATE: Update Username and IsApproved only (Email, FirstName and LastName are read-only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                TempData["Message"] = "Invalid user ID.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    if (user == null)
                    {
                        TempData["Message"] = "User not found.";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index");
                    }

                    // Check if new username conflicts with other users
                    var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);

                    if (existingUserByUsername != null && existingUserByUsername.Id != user.Id)
                    {
                        ModelState.AddModelError("UserName", "Username already exists.");
                        return View(model);
                    }

                    // Update only Username and IsApproved (Email, FirstName and LastName cannot be changed)
                    user.UserName = model.UserName;
                    user.IsApproved = model.IsApproved;
                    // Note: Email, FirstName and LastName remain unchanged

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["Message"] = $"User '{user.FirstName} {user.LastName}' updated successfully!";
                        TempData["MessageType"] = "success";
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error updating user: " + ex.Message);
                }
            }
            return View(model);
        }

        // CHANGE PASSWORD: Show change password form
        public async Task<IActionResult> ChangePassword(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    TempData["Message"] = "User not found.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("Index");
                }

                var model = new ChangePasswordViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error loading user: " + ex.Message;
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

        // CHANGE PASSWORD: Admin can change user password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                    if (user == null)
                    {
                        TempData["Message"] = "User not found.";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Index");
                    }

                    // Remove current password and add new one
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    if (!removePasswordResult.Succeeded)
                    {
                        foreach (var error in removePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }

                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (addPasswordResult.Succeeded)
                    {
                        TempData["Message"] = $"Password for user '{user.FirstName} {user.LastName}' changed successfully!";
                        TempData["MessageType"] = "success";
                        return RedirectToAction("Index");
                    }

                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error changing password: " + ex.Message);
                }
            }
            return View(model);
        }

        // BONUS: Toggle user approval status
        [HttpPost]
        public async Task<IActionResult> ToggleApproval(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                user.IsApproved = !user.IsApproved;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var status = user.IsApproved ? "approved" : "unapproved";
                    return Json(new { success = true, message = $"User {status} successfully!", isApproved = user.IsApproved });
                }
                else
                {
                    return Json(new { success = false, message = "Error updating user approval status." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            // Prevent deleting Admins
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["Message"] = "Admins cannot be deleted.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            return View(user); // Goes to Views/AccountManagement/Delete.cshtml
        }

        // DELETE: Confirm delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            // Prevent deleting Admins
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["Message"] = "Admins cannot be deleted.";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = $"User '{user.FirstName} {user.LastName}' deleted successfully!";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["Message"] = "Error deleting user.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }
    }
}