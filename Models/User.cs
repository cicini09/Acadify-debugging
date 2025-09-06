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
public class User
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = null!;

    [Column("email")]
    [Required(ErrorMessage = "Email address is required")]
    [StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;

    [JsonIgnore]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("role")]
    [Range(1, 3, ErrorMessage = "A valid role must be selected.")]
    public Role Role { get; set; }

    [Column("profile_picture")]
    [StringLength(255, ErrorMessage = "Profile picture URL cannot exceed 255 characters")]
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
