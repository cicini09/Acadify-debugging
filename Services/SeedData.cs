using Microsoft.AspNetCore.Identity;
using Student_Performance_Tracker.Enums;

namespace Student_Performance_Tracker.Services
{
    public static class SeedData
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Teacher = "Teacher";
            public const string Student = "Student";

            public static readonly string[] AllRoles = { Admin, Teacher, Student };

            public static IdentityRole<int>[] GetRoleEntities()
            {
                return new[]
                {
                    new IdentityRole<int> { Name = Admin, NormalizedName = Admin.ToUpperInvariant() },
                    new IdentityRole<int> { Name = Teacher, NormalizedName = Teacher.ToUpperInvariant() },
                    new IdentityRole<int> { Name = Student, NormalizedName = Student.ToUpperInvariant() }
                };
            }
        }
    }
}
