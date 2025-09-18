namespace ASI.Basecode.Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public int? TeacherId { get; set; }
        public string JoinCode { get; set; } = null!;
        public short Units { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User? Teacher { get; set; }
        public virtual User? Creator { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}