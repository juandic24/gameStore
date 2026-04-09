
namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required List<String> Platforms { get; set; }

    public required int Stock { get; set; }
    public Genre? Genre { get; set; }

    public int GenreId { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    

}
