
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{


    //Constants
    const string GetGameEndPointName = "GetGame";


    public static void MapGamesEndPoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");
        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var games = await dbContext.Games
                .Include(game => game.Genre)
                .AsNoTracking()
                .ToListAsync();

            return games.Select(game => new GameSummaryDto(
                game.Id,
                game.Name,
                game.Description ?? string.Empty,
                game.Platforms,
                game.Stock,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            ));
        });

        // GET /games/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.Description ?? string.Empty,
                    game.Platforms,
                    game.Stock,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                ) 
            );
        })
        .WithName(GetGameEndPointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Description = newGame.Description,
                Platforms = newGame.Platforms,
                Stock = newGame.Stock,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate 
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.Description ?? string.Empty,
                game.Platforms,
                game.Stock,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndPointName, new { id = gameDto.Id }, gameDto);
        })
        .RequireAuthorization("AdminOnly");

        // PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame,
                                 GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.Description = updatedGame.Description;
            existingGame.Platforms = updatedGame.Platforms;
            existingGame.Stock = updatedGame.Stock;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        })
        .RequireAuthorization("AdminOnly");

        //DELETE /games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {

            await dbContext.Games
                            .Where(game => game.Id == id)
                            .ExecuteDeleteAsync();
    
            return Results.NoContent();

        })
        .RequireAuthorization("AdminOnly");

    }
}
