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

            builder.HasIndex(c => c.CourseCode)
                .IsUnique();

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(c => c.CourseCode)
                .HasColumnName("course_code")
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(c => c.CourseName)
                .HasColumnName("course_name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnName("description")
                .HasColumnType("TEXT");

            builder.Property(c => c.Units)
                .HasColumnName("units") 
                .HasColumnType("SMALLINT")
                .IsRequired();

            builder.Property(c => c.YearLevel)
                .HasColumnName("year_level")
                .HasColumnType("SMALLINT")
                .IsRequired();

            builder.Property(c => c.AvailableSemester)
                .HasColumnName("available_semester")
                .HasColumnType("SMALLINT")
                .IsRequired();

            builder.Property(c => c.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("BOOLEAN")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("NOW()");
        }
    }
}