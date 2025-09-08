namespace Student_Performance_Tracker.Models;

public class Grade
{
    public long Id { get; set; }
    public long EnrollmentId { get; set; }
    public decimal? MidtermGrade { get; set; }
    public decimal? FinalGrade { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // Navigation: each grade is tied to one enrollment
    public virtual Enrollment Enrollment { get; set; } = null!;
}
