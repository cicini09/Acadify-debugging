namespace ASI.Basecode.Data.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal? MidtermGrade { get; set; }
        public decimal? FinalGrade { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}   