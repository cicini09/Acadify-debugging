using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Student_Performance_Tracker.Data;

namespace Student_Performance_Tracker.Services
{
    public class RoleSeeder
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<RoleSeeder> _logger;

        public RoleSeeder(AppDbContext context, RoleManager<IdentityRole<int>> roleManager, ILogger<RoleSeeder> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedRolesAsync()
        {
            try
            {
                // Ensure database is created
                await _context.Database.EnsureCreatedAsync();

                // Check if roles already exist
                if (await _roleManager.Roles.AnyAsync())
                {
                    _logger.LogInformation("Roles already exist. Skipping role seeding.");
                    return;
                }

                _logger.LogInformation("Starting role seeding...");

                // Define the roles to create
                var roles = new[]
                {
                    new IdentityRole<int> 
                    { 
                        Name = "Admin", 
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole<int> 
                    { 
                        Name = "Teacher", 
                        NormalizedName = "TEACHER",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole<int> 
                    { 
                        Name = "Student", 
                        NormalizedName = "STUDENT",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    }
                };

                // Create each role
                foreach (var role in roles)
                {
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Successfully created role: {RoleName}", role.Name);
                    }
                    else
                    {
                        _logger.LogError("Failed to create role: {RoleName}. Errors: {Errors}", 
                            role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                _logger.LogInformation("Role seeding completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding roles.");
                throw;
            }
        }

        public async Task<bool> RolesExistAsync()
        {
            return await _roleManager.Roles.AnyAsync();
        }

        public async Task<List<IdentityRole<int>>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
