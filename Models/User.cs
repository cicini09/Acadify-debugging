using Microsoft.AspNetCore.Identity;
using Student_Performance_Tracker.Enums;

namespace Student_Performance_Tracker.Models;

public class User : IdentityUser<long>
{
    public string Name { get; set; } = null!;
    public Role Role { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    // Navigation properties remain the same
    public virtual ICollection<Course> CoursesTeaching { get; set; } = new List<Course>();
    public virtual ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}