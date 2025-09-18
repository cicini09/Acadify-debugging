using Microsoft.AspNetCore.Identity;
using Student_Performance_Tracker.Models;
using Student_Performance_Tracker.ViewModels.Account;

namespace Student_Performance_Tracker.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;


    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
    {
        var user = new User
        {
            UserName = model.Email,
            Name = model.Name,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return AuthResult.Failure(result.Errors.Select(e => e.Description));
        }

        await _userManager.AddToRoleAsync(user, model.Role);
        return AuthResult.Success();
    }

    public async Task<SignInAuthResult> LoginAsync(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return SignInAuthResult.Failed("Invalid login attempt.");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            isPersistent: model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return SignInAuthResult.Success();
        }

        if (result.IsLockedOut)
        {
            return SignInAuthResult.LockedOut();
        }

        return SignInAuthResult.Failed("Invalid login attempt.");
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new List<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<ForgotPasswordResult> SendPasswordResetTokenAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!(user == null))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _emailService.SendPasswordResetEmailAsync(user.Email!, user.Name, token);
            }

            // For security, don't reveal if user exists or not
            // Always return success to prevent email enumeration attacks
            return ForgotPasswordResult.Success();
        }
        catch (Exception)
        {
            return ForgotPasswordResult.EmailFailed("Failed to send password reset email. Please try again later.");
        }
    }

    public async Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return AuthResult.Failure(new[] { "Invalid password reset request" });
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            return AuthResult.Success();
        }

        return AuthResult.Failure(result.Errors.Select(e => e.Description));
    }
}