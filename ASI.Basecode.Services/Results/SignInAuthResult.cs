namespace ASI.Basecode.Services.Results;

public class SignInAuthResult
{
    public bool Succeeded { get; set; }
    public bool IsLockedOut { get; set; }
    public string? ErrorMessage { get; set; }

    public static SignInAuthResult Success() => new() { Succeeded = true };
    public static SignInAuthResult Failed(string message) => new() { Succeeded = false, ErrorMessage = message };
    public static SignInAuthResult LockedOut() => new() { IsLockedOut = true };
}


