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

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasColumnType("bigint");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name")
            .HasColumnType("varchar(100)");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("email")
            .HasColumnType("varchar(255)");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasColumnName("password_hash")
            .HasColumnType("text");

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()   // store enum as string
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
    }
}

