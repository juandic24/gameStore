
using GameStore.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Api.Services;
public class PasswordHasherService()
{
    private readonly PasswordHasher<User> passwordHasher = new();
    public string HashPassword(User user, string password)
    {
        return passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

        return result == PasswordVerificationResult.Success
            || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
    
}