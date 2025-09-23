using System.ComponentModel.DataAnnotations;

namespace Student_Performance_Tracker.ViewModels.AccountManagement
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        // Read-only fields for display purposes
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Approve User Account")]
        public bool IsApproved { get; set; }
    }
}
