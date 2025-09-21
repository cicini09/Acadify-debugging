namespace ASI.Basecode.Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public short Units { get; set; }        
        public short YearLevel { get; set; }
        public short AvailableSemester { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}