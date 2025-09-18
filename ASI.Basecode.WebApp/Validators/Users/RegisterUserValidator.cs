using FluentValidation;
using Student_Performance_Tracker.ViewModels.Users;

namespace Student_Performance_Tracker.Validators.Users;

public class RegisterUserValidator : AbstractValidator<RegisterUserViewModel>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Password)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}
