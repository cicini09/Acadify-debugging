using System.ComponentModel.DataAnnotations;
using Student_Performance_Tracker.Enums;

namespace Student_Performance_Tracker.ViewModels.Users;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required")]
    [StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; set; }
}
