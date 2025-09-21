using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Data.Configurations
{
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
                .UseIdentityColumn();

            builder.Property(g => g.EnrollmentId)
                .HasColumnName("enrollment_id")
                .IsRequired();

            builder.Property(g => g.MidtermGrade)
                .HasColumnName("midterm_grade")
                .HasColumnType("DECIMAL(5,1)");

            builder.Property(g => g.FinalGrade)
                .HasColumnName("final_grade")
                .HasColumnType("DECIMAL(5,1)");

            builder.Property(g => g.Remarks)
                .HasColumnName("remarks")
                .HasColumnType("TEXT");

            builder.Property(g => g.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("NOW()");

            builder.Property(g => g.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("TIMESTAMP");

            // One-to-one with Enrollment
            builder.HasOne(g => g.Enrollment)
                .WithOne(e => e.Grade)
                .HasForeignKey<Grade>(g => g.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}