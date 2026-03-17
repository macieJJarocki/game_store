using System.Net;
using GameStore.Api.Dtos;

const string GetGameEndpointName = "GatGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
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
    ),
];


// GET /games
app.MapGet("/games", () => games);

// GET /games/1

app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

// POST /games/4
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/4
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
    return Results.NoContent();
});

// DELETE /games/4
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll((game) => game.Id == id);
    return Results.NoContent();
});

app.Run();
