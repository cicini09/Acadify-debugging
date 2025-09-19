namespace ASI.Basecode.Data.Repositories;

using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class AuthRepository : IAuthRepository
{
    private readonly SignInManager<User> _signInManager;

    public AuthRepository(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<(bool Succeeded, bool IsLockedOut)> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        return (result.Succeeded, result.IsLockedOut);
    }

    public Task SignOutAsync() => _signInManager.SignOutAsync();
}


