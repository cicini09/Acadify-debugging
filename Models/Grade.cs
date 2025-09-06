using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Student_Performance_Tracker.Models;

[Table("grades")]
[Index(nameof(EnrollmentId), IsUnique = true)]
public class Grade {
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("enrollment_id")]
    [Required(ErrorMessage = "Enrollment ID is required")]
    public long EnrollmentId { get; set; }

    [Column("midterm_grade")]
    [Precision(5, 2)]
    [Range(0, 100, ErrorMessage = "Midterm grade must be between 0 and 100")]
    public decimal? MidtermGrade { get; set; }

    [Column("final_grade")]
    [Precision(5, 2)]
    [Range(0, 100, ErrorMessage = "Final grade must be between 0 and 100")]
    public decimal? FinalGrade { get; set; }

    [Column("remarks")]
    [StringLength(255, ErrorMessage = "Remarks cannot exceed 255 characters")]
    public string? Remarks { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation: each grade is tied to one enrollment
    [ForeignKey("EnrollmentId")]
    [InverseProperty("Grade")]
    public virtual Enrollment Enrollment { get; set; } = null!;
}
