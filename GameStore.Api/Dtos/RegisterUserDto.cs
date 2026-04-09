using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record RegisterUserDto (
    [Required, EmailAddress]string Email, 
    [Required, MinLength(6)]string Password);

