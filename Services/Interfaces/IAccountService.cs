using Student_Performance_Tracker.ViewModels.Users;

public interface IAccountService
{
    Task<AuthResult> RegisterAsync(RegisterViewModel model);
    Task<SignInAuthResult> LoginAsync(LoginViewModel model);
    Task SignOutAsync();
    Task<ForgotPasswordResult> SendPasswordResetTokenAsync(string email);
    Task<IList<String>> GetUserRolesAsync(string email);
}