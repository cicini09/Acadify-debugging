using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("courses");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.JoinCode)
                .IsUnique();

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(c => c.CourseName)
                .HasColumnName("course_name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnName("description")
                .HasColumnType("TEXT");

            builder.Property(c => c.TeacherId)
                .HasColumnName("teacher_id");

            builder.Property(c => c.JoinCode)
                .HasColumnName("join_code")
                .HasColumnType("VARCHAR(10)")
                .IsRequired();

            builder.Property(c => c.Units)
                .HasColumnName("units")
                .HasColumnType("SMALLINT")
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("NOW()");

            // Relationships
            builder.HasOne(c => c.Teacher)
                .WithMany(u => u.CoursesTeaching)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}