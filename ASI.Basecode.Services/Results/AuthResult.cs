namespace ASI.Basecode.Services.Results;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    public static AuthResult Success() => new() { Succeeded = true };
    public static AuthResult Failure(IEnumerable<string> errors) => new() { Succeeded = false, Errors = errors };
}


