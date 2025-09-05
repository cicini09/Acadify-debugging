using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Student_Performance_Tracker.Enums;
using System.Text.Json.Serialization;
namespace Student_Performance_Tracker.Models;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [JsonIgnore]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("role")]
    public Role Role { get; set; }

    [Column("profile_picture")]
    [StringLength(255)]
    public string? ProfilePicture { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;


    // Navigation: A teacher can be assigned to courses
    [InverseProperty("AssignedTeacher")]
    public virtual ICollection<Course> CoursesTeaching { get; set; } = new List<Course>();

    // Navigation: A user (teacher/admin) can create courses
    [InverseProperty("Creator")]
    public virtual ICollection<Course> CreatedCourses { get; set; } = new List<Course>();

    // Navigation: A student has enrollments
    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
