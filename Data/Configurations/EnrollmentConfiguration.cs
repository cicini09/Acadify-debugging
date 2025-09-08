using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Performance_Tracker.Models;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("enrollments");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique()
            .HasDatabaseName("enrollments_student_id_course_id_key");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint");

        builder.Property(e => e.StudentId)
            .IsRequired()
            .HasColumnName("student_id")
            .HasColumnType("bigint");

        builder.Property(e => e.CourseId)
            .IsRequired()
            .HasColumnName("course_id")
            .HasColumnType("bigint");

        builder.Property(e => e.EnrollmentDate)
            .HasColumnName("enrolled_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");

        // Relationships

        // Student (User -> Enrollments)
        builder.HasOne(e => e.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Course (Course -> Enrollments)
        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Grade (1-to-1 relationship)
        builder.HasOne(e => e.Grade)
            .WithOne(g => g.Enrollment)
            .HasForeignKey<Grade>(g => g.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
