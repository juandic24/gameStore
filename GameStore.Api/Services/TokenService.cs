using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStore.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.Api.Services;

public class TokenService(IConfiguration configuration)
{
    public string CreateToken(User user)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var key = jwtSettings["Key"]
            ?? throw new InvalidOperationException("JWT Key is missing.");

        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is missing.");

        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience is missing.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
