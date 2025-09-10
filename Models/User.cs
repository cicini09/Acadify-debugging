using Microsoft.AspNetCore.Identity;
using Student_Performance_Tracker.Enums;

namespace Student_Performance_Tracker.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public Role Role { get; set; } = Role.None;
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Course> CoursesTeaching { get; set; } = new List<Course>();
        public virtual ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}