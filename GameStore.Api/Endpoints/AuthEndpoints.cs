using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using GameStore.Api.Services;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth");

        // /auth/service
        group.MapPost("/register", async (
            RegisterUserDto registerDto,
            GameStoreContext dbContext,
            PasswordHasherService passwordHasher,
            TokenService tokenService) =>
        {
            var normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();

            var userExists = await dbContext.Users
                .AnyAsync(u => u.Email == normalizedEmail);

            if (userExists)
            {
                return Results.Conflict(new { message = "Email already registered." });
            }

            var isFirstUser = !await dbContext.Users.AnyAsync();

            var user = new User
            {
                Email = normalizedEmail,
                PasswordHash = string.Empty,
                Role = isFirstUser ? "Admin" : "User"
            };

            user.PasswordHash = passwordHasher.HashPassword(user, registerDto.Password);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var token = tokenService.CreateToken(user);

            return Results.Ok(new AuthResponseDto(token, user.Email, user.Role));
        });

        group.MapPost("/login", async (
            LoginUserDto loginDto,
            GameStoreContext dbContext,
            PasswordHasherService passwordHasher,
            TokenService tokenService) =>
        {
            var normalizedEmail = loginDto.Email.Trim().ToLower();

            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            var isPasswordValid = passwordHasher.VerifyPassword(
                user,
                user.PasswordHash,
                loginDto.Password);

            if (!isPasswordValid)
            {
                return Results.Unauthorized();
            }

            var token = tokenService.CreateToken(user);

            return Results.Ok(new AuthResponseDto(token, user.Email, user.Role));
        });
    }
}
