namespace GameStore.Api.Dtos;

public record GameDetailsDto(
    int Id,
    string Name,
    string Description,
    List<String> Platforms,
    int Stock,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate

);
