namespace ASI.Basecode.Data.Repositories;

using ASI.Basecode.Data.Models;
using System.Threading.Tasks;

public interface IAuthRepository
{
    Task<(bool Succeeded, bool IsLockedOut)> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOutAsync();
}


