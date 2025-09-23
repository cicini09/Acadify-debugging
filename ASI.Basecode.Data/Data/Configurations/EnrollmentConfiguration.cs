using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("enrollments");

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => new { e.StudentId, e.ClassId })
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            builder.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .IsRequired();

            builder.Property(e => e.EnrolledAt)
                .HasColumnName("enrolled_at")
                .HasColumnType("TIMESTAMPTZ")
                .HasDefaultValueSql("NOW()");
            

            // Relationships
            builder.HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Class)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}