namespace Student_Performance_Tracker.Models
{
    public class Course
    {
        public long Id { get; set; }
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public long? TeacherId { get; set; }
        public string JoinCode { get; set; } = null!;
        public short Units { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User? Teacher { get; set; }
        public virtual User? Creator { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}