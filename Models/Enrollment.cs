namespace Student_Performance_Tracker.Models;

public class Enrollment
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual User Student { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Grade? Grade { get; set; }   // One-to-one with Grade
}
        