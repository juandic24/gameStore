using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Range(1,50)] int GenreId,
    string Description,
    [Required] List<String> Platforms,
    [Required] int Stock,
    [Required][Range(1,500)]decimal Price,
    [Required]DateOnly ReleaseDate
);
