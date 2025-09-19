namespace ASI.Basecode.Services.Interfaces;

using ASI.Basecode.Services.DTOs;
using ASI.Basecode.Services.Results;

public interface IAccountService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<SignInAuthResult> LoginAsync(LoginRequest request);
    Task SignOutAsync();
    Task<IList<string>> GetUserRolesAsync(string email);
    Task<ForgotPasswordResult> SendPasswordResetTokenAsync(string email);
    Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword);
}


