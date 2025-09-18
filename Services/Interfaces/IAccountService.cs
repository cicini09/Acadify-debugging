using Student_Performance_Tracker.ViewModels.Users;

public interface IAccountService
{
    Task<AuthResult> RegisterAsync(RegisterViewModel model);
    Task<SignInAuthResult> LoginAsync(LoginViewModel model);
    Task SignOutAsync();
    Task<IList<String>> GetUserRolesAsync(string email);
    Task<ForgotPasswordResult> SendPasswordResetTokenAsync(string email);
    Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword);
}