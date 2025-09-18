namespace Student_Performance_Tracker.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string userName, string resetToken);
}