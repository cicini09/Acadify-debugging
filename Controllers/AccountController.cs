using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Student_Performance_Tracker.ViewModels.Users;

namespace Student_Performance_Tracker.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;


    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // GET: /Account/Register
    [HttpGet]
    public ViewResult Register() => View();

    // GET: /Account/Login
    [HttpGet]
    public ViewResult Login(string? returnUrl = null)
    {
        ViewData[nameof(returnUrl)] = returnUrl;
        return View();
    }

    // GET: /Account/ForgotPassword
    [HttpGet]
    public ViewResult ForgotPassword() => View();

    // GET: /Account/ResetPassword
    [HttpGet]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login");
        }

        var model = new ResetPasswordViewModel{Email = email, Token = token};
        return View(model);
    }




    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.RegisterAsync(model);

        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Account");
        }

        // Add errors to ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }

        return View(model);
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return ViewWithReturnUrl(model, returnUrl);
        }

        var result = await _accountService.LoginAsync(model);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return await RedirectBasedOnRoleAsync(model.Email);
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "This account has been locked due to multiple failed login attempts. Please try again in 15 minutes.");
            return ViewWithReturnUrl(model, returnUrl);
        }

        ModelState.AddModelError("", result.ErrorMessage ?? "Invalid login attempt.");
        return ViewWithReturnUrl(model, returnUrl);
    }

    // POST: /Account/ForgotPassword
    [HttpPost] 
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.SendPasswordResetTokenAsync(model.Email);

        // Always show success message for security (prevents email enumeration attacks)
        // Even if the email doesn't exist, we show the same message
        ViewBag.Message = "If an account with that email exists, a password reset link has been sent to your email address.";
        ViewBag.Email = model.Email; // Optional: to show which email was used

        return View("ForgotPasswordConfirmation");
    }

    // POST: /Account/ResetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.ResetPasswordAsync(model.Email, model.Token, model.Password);

        if (result.Succeeded)
        {
            ViewBag.Message = "Your password has been reset successfully. You can now log in with your new password";
            return View("ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }

        return View(model);
    }

    // POST: /Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    
    {
        await _accountService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }



    // Helper Methods
    private ViewResult ViewWithReturnUrl<T>(T model, string? returnUrl)
    {
        ViewData[nameof(returnUrl)] = returnUrl;
        return View(model);
    }

    private async Task<IActionResult> RedirectBasedOnRoleAsync(string email)
    {
        var roles = await _accountService.GetUserRolesAsync(email);
        
        return roles.FirstOrDefault() switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "Teacher" => RedirectToAction("Index", "Teacher"),
            "Student" => RedirectToAction("Index", "Student"),
            _ => RedirectToAction("Index", "Home")
        };
    }
}