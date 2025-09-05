using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Student_Performance_Tracker.Models;

[Table("courses")]
[Index("JoinCode", Name = "courses_join_code_key", IsUnique = true)]
public partial class Course
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("course_name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("teacher_id")]
    public long? TeacherId { get; set; }   // nullable because of ON DELETE SET NULL

    [Column("join_code")]
    [StringLength(10)]
    public string JoinCode { get; set; } = null!;

    [Column("units")]
    public short Units { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }   // Admin or Teacher  

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    // Navigation: Teacher assigned to course
    [ForeignKey("TeacherId")]
    [InverseProperty("CoursesTeaching")]
    public virtual User? AssignedTeacher { get; set; }

    // Navigation: Creator (Admin or Teacher)
    [ForeignKey("CreatedBy")]
    [InverseProperty("CreatedCourses")]
    public virtual User? Creator { get; set; }

    // Navigation: Students enrolled
    [InverseProperty("Course")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
