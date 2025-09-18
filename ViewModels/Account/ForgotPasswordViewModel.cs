using System.ComponentModel.DataAnnotations;

namespace Student_Performance_Tracker.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = null!;
}
