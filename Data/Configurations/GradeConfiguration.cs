using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Performance_Tracker.Models;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("grades");

        builder.HasKey(g => g.Id);

        builder.HasIndex(g => g.EnrollmentId)
            .IsUnique();

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .HasColumnType("bigint");

        builder.Property(g => g.EnrollmentId)
            .IsRequired()
            .HasColumnName("enrollment_id")
            .HasColumnType("bigint");

        builder.Property(g => g.MidtermGrade)
            .HasColumnName("midterm_grade")
            .HasColumnType("decimal(5,2)");

        builder.Property(g => g.FinalGrade)
            .HasColumnName("final_grade")
            .HasColumnType("decimal(5,2)");

        builder.Property(g => g.Remarks)
            .HasColumnName("remarks")
            .HasColumnType("text");

        builder.Property(g => g.CreationDate)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(g => g.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp without time zone");

        // Relationships

        // Grade ↔ Enrollment (1-to-1)
        builder.HasOne(g => g.Enrollment)
            .WithOne(e => e.Grade)
            .HasForeignKey<Grade>(g => g.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
