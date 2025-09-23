using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Data.Configurations;

namespace ASI.Basecode.Data.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove unused Identity tables
            modelBuilder.Ignore<IdentityUserLogin<int>>();

            // Override Identity table names for consistency
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("roles");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");

            // Apply configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new GradeConfiguration());
        }
    }
}