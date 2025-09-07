using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Student_Performance_Tracker.Enums;
using System.Text.Json.Serialization;
namespace Student_Performance_Tracker.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;


    // Navigation: A teacher can be assigned to courses
    public virtual ICollection<Course> CoursesTeaching { get; set; } = new List<Course>();

    // Navigation: A user (teacher/admin) can create courses
    public virtual ICollection<Course> CreatedCourses { get; set; } = new List<Course>();

    // Navigation: A student has enrollments
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
