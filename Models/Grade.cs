namespace Student_Performance_Tracker.Models
{
    public class Grade
    {
        public long Id { get; set; }
        public long EnrollmentId { get; set; }
        public decimal? MidtermGrade { get; set; }
        public decimal? FinalGrade { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}