public class ForgotPasswordResult
{
    public bool UserExists { get; set; }
    public bool EmailSent { get; set; }
    public string? ErrorMessage { get; set; }
    
    
    public static ForgotPasswordResult Success() => new() { UserExists = true, EmailSent = true };
    public static ForgotPasswordResult UserNotFound() => new() { UserExists = false };
    public static ForgotPasswordResult EmailFailed(string message) => new() { UserExists = true, EmailSent = false, ErrorMessage = message };
}