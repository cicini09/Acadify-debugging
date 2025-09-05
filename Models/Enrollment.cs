using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Student_Performance_Tracker.Models;

[Table("enrollments")]
[Index("StudentId", "CourseId", Name = "enrollments_student_id_course_id_key", IsUnique = true)]
public partial class Enrollment
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("student_id")]
    public long StudentId { get; set; }

    [Column("course_id")]
    public long CourseId { get; set; }

    [Column("enrolled_at", TypeName = "timestamp without time zone")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual User Student { get; set; } = null!;

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course Course { get; set; } = null!;

    // Navigation: link to Grade
    [InverseProperty("Enrollment")]
    public virtual Grade? Grade { get; set; }
}
