using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Student_Performance_Tracker.Models;
using Student_Performance_Tracker.Enums;

namespace Student_Performance_Tracker.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", t =>
            {
                t.HasCheckConstraint("CK_users_role", "role IN ('Admin', 'Teacher', 'Student')");
            });

            builder.Property(u => u.Id).HasColumnName("id");
            builder.Property(u => u.Email).HasColumnName("email");
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash");

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(
                    v => v.ToString(),  
                    v => (Role)Enum.Parse(typeof(Role), v))  
                .IsRequired();

            builder.Property(u => u.ProfilePicture)
                .HasColumnName("profile_picture")
                .HasColumnType("VARCHAR(255)");

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("NOW()");

            // Keep Identity columns mapped to avoid EF warnings; do not ignore them
            builder.Property(u => u.UserName).HasColumnName("user_name");
            builder.Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name");
            builder.Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            builder.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            builder.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            builder.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            builder.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            builder.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            builder.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            builder.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
        }
    }
}