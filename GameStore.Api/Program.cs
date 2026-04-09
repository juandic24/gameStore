using System.Text;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.Services.AddScoped<PasswordHasherService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.AddGameStoreDb();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapGamesEndPoints();
app.MapGenresEndpoints();

app.MigrateDb();
app.Run();
