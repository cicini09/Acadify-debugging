using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Student_Performance_Tracker.Models;

[Table("enrollments")]
[Index("StudentId", "CourseId", Name = "enrollments_student_id_course_id_key", IsUnique = true)]
public class Enrollment
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("student_id")]
    [Required(ErrorMessage = "Student ID is required")]
    public long StudentId { get; set; }

    [Column("course_id")]
    [Required(ErrorMessage = "Course ID is required")]
    public long CourseId { get; set; }

    [Column("enrolled_at", TypeName = "timestamp without time zone")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    public virtual User Student { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    // Navigation: link to Grade
    [InverseProperty("Enrollment")]
    public virtual Grade? Grade { get; set; }
}
