using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Performance_Tracker.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
   public void Configure(EntityTypeBuilder<User> builder)
   {
       // Keep the email unique index - Identity doesn't create this automatically
       builder.HasIndex(u => u.Email)
           .IsUnique()
           .HasDatabaseName("IX_AspNetUsers_Email");

       // Configure ONLY your custom properties, not Identity properties
       builder.Property(u => u.Name)
           .IsRequired()
           .HasMaxLength(100)
           .HasColumnName("name")
           .HasColumnType("varchar(100)");

       builder.Property(u => u.Role)
           .IsRequired()
           .HasConversion<string>()
           .HasColumnName("role")
           .HasColumnType("varchar(20)");

       builder.Property(u => u.ProfilePicture)
           .HasMaxLength(2048)
           .HasColumnName("profile_picture")
           .HasColumnType("text");

       builder.Property(u => u.RegistrationDate)
           .HasColumnName("created_at")
           .HasColumnType("timestamp without time zone")
           .HasDefaultValueSql("NOW()");

       // Navigation properties
       builder.HasMany(u => u.CoursesTeaching)
           .WithOne(c => c.AssignedTeacher)
           .HasForeignKey(c => c.TeacherId)
           .OnDelete(DeleteBehavior.SetNull);

       builder.HasMany(u => u.CreatedCourses)
           .WithOne(c => c.Creator)
           .HasForeignKey(c => c.CreatedBy)
           .OnDelete(DeleteBehavior.SetNull);

       builder.HasMany(u => u.Enrollments)
           .WithOne(e => e.Student)
           .HasForeignKey(e => e.StudentId)
           .OnDelete(DeleteBehavior.Cascade);
   }
}