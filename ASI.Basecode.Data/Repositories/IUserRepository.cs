namespace ASI.Basecode.Data.Repositories;

using ASI.Basecode.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(User user, string password);
    Task AddToRoleAsync(User user, string role);
    Task<User?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(User user);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(User user, string token, string newPassword);
}


