using Microsoft.EntityFrameworkCore;
using Student_Performance_Tracker.Models;

namespace Student_Performance_Tracker.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === Users ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");
                entity.Property(e => e.RegistrationDate).HasDefaultValueSql("now()");
                entity.Property(e => e.Role).HasConversion<string>();
            });

            // === Courses ===
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("courses_pkey");
                entity.Property(e => e.CreationDate).HasDefaultValueSql("now()");

                entity.HasOne(d => d.AssignedTeacher)
                      .WithMany(p => p.CoursesTeaching)
                      .HasForeignKey(d => d.TeacherId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("courses_teacher_id_fkey");

                entity.HasOne(d => d.Creator)
                      .WithMany(p => p.CreatedCourses)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("courses_created_by_fkey");
            });

            // === Enrollments ===
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("enrollments_pkey");
                entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Course)
                      .WithMany(p => p.Enrollments)
                      .HasForeignKey(d => d.CourseId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("enrollments_course_id_fkey");

                entity.HasOne(d => d.Student)
                      .WithMany(p => p.Enrollments)
                      .HasForeignKey(d => d.StudentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("enrollments_student_id_fkey");
            });

            // === Grades ===
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("grades_pkey");
                entity.Property(e => e.CreationDate).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Enrollment)
                      .WithOne(p => p.Grade)
                      .HasForeignKey<Grade>(d => d.EnrollmentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("grades_enrollment_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
