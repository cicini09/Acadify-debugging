namespace ASI.Basecode.Resources.Messages;

public static class ValidationMessages
{
    // Password Validation
    public const string PasswordRequireUppercase = "Password must contain at least one uppercase letter";
    public const string PasswordRequireLowercase = "Password must contain at least one lowercase letter";
    public const string PasswordRequireNumber = "Password must contain at least one number";
    public const string PasswordRequireSpecialChar = "Password must contain at least one special character";
    
    // Email Validation
    public const string EmailRequired = "Email is required";
    public const string EmailInvalidFormat = "Invalid email format";
    
    // General Validation
    public const string FieldRequired = "This field is required";
    public const string FieldTooShort = "This field is too short";
    public const string FieldTooLong = "This field is too long";
}
