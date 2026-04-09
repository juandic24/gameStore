using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record LoginUserDto (
    [Required, EmailAddress]string Email, 
    [Required]string Password);
