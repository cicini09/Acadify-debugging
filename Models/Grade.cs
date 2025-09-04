﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Student_Performance_Tracker.Models;

[Table("grades")]
[Index(nameof(EnrollmentId), IsUnique = true)]
public partial class Grade{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("enrollment_id")]
    public long EnrollmentId { get; set; }

    [Column("midterm")]
    [Precision(5, 2)]
    public decimal? Midterm { get; set; }

    [Column("final")]
    [Precision(5, 2)]
    public decimal? Final { get; set; }

    [Column("remarks")]
    public string? Remarks { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation: each grade is tied to one enrollment
    [ForeignKey("EnrollmentId")]
    [InverseProperty("Grade")]
    public virtual Enrollment Enrollment { get; set; } = null!;
}
