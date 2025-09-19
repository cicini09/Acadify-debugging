namespace ASI.Basecode.Services.Implementation;

using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.DTOs;
using ASI.Basecode.Services.Results;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IEmailService _emailService;

    public AccountService(IUserRepository userRepository, IAuthRepository authRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _authRepository = authRepository;
        _emailService = emailService;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Name = request.Name,
            Email = request.Email
        };

        var (succeeded, errors) = await _userRepository.CreateUserAsync(user, request.Password);
        if (!succeeded)
        {
            return AuthResult.Failure(errors);
        }

        await _userRepository.AddToRoleAsync(user, request.Role);
        return AuthResult.Success();
    }

    public async Task<SignInAuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return SignInAuthResult.Failed("Invalid login attempt.");
        }

        var (succeeded, isLockedOut) = await _authRepository.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: request.RememberMe,
            lockoutOnFailure: true);

        if (succeeded)
        {
            return SignInAuthResult.Success();
        }

        if (isLockedOut)
        {
            return SignInAuthResult.LockedOut();
        }

        return SignInAuthResult.Failed("Invalid login attempt.");
    }

    public async Task SignOutAsync()
    {
        await _authRepository.SignOutAsync();
    }

    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            return new List<string>();
        }

        return await _userRepository.GetRolesAsync(user);
    }

    public async Task<ForgotPasswordResult> SendPasswordResetTokenAsync(string email)
    {
        try
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (!(user == null))
            {
                var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
                await _emailService.SendPasswordResetEmailAsync(user.Email!, user.Name, token);
            }

            return ForgotPasswordResult.Success();
        }
        catch (Exception)
        {
            return ForgotPasswordResult.EmailFailed("Failed to send password reset email. Please try again later.");
        }
    }

    public async Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            return AuthResult.Failure(new[] { "Invalid password reset request" });
        }

        var (succeeded, errors) = await _userRepository.ResetPasswordAsync(user, token, newPassword);
        if (succeeded)
        {
            return AuthResult.Success();
        }

        return AuthResult.Failure(errors);
    }
}


