using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Performance_Tracker.Models;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.JoinCode)
            .IsUnique()
            .HasDatabaseName("courses_join_code_key");

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasColumnType("bigint");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("course_name")
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(c => c.TeacherId)
            .HasColumnName("teacher_id")
            .HasColumnType("bigint");

        builder.Property(c => c.JoinCode)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("join_code")
            .HasColumnType("varchar(10)");

        builder.Property(c => c.Units)
            .IsRequired()
            .HasColumnName("units")
            .HasColumnType("smallint");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by")
            .HasColumnType("bigint");

        builder.Property(c => c.CreationDate)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");

        // Relationships

        // Assigned teacher (User -> CoursesTeaching)
        builder.HasOne(c => c.AssignedTeacher)
            .WithMany(u => u.CoursesTeaching)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Creator (User -> CreatedCourses)
        builder.HasOne(c => c.Creator)
            .WithMany(u => u.CreatedCourses)
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Enrollments
        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
