using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Performance_Tracker.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("users_email_key");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("password_hash");

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>(); // store enum as int

        builder.Property(u => u.ProfilePicture)
            .HasMaxLength(255);

        builder.Property(u => u.RegistrationDate)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");
    }
}
