using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GatGame";

    private static readonly List<GameDto> games = [
        new (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateOnly(1992, 11, 4)
            ),
        new (
            2,
            "Final Fantasy VII Rebirth",
            "RPG",
            69.59M,
            new DateOnly(1999, 4, 8)
            ),
        new (
            3,
            "Astro Bot",
            "Platformer",
            43.49M,
            new DateOnly(2000, 7, 23)
            )
    ];

    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games
            .Include(game => game.Genre)
            .Select(game => new GameDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate))
            .ToListAsync());

        // GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate));
        }).WithName(GetGameEndpointName);

        // POST /games/4
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            // DTO 
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/4
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
            return Results.NoContent();
        });

        // DELETE /games/4
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll((game) => game.Id == id);
            return Results.NoContent();
        });
    }
}
