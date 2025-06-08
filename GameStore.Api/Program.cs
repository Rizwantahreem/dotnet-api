using GameStore.Api.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// configuration for request pipeline
var app = builder.Build();
string GetRouteName = "GetGame";

List<GameDto> games = [
    new (1, "Street Figther II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
    new (2, "Finally Fantasy XIV", "Roleplaying", 59.9M, new DateOnly(2010, 9, 30)),
    new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
];

// GET /games
app.MapGet("games", () => games);

// GET /games/:id
app.MapGet("games/{id}", (int id) => games.Find(game => game.id == id))
    .WithName(GetRouteName);

// POST /games
app.MapPost("games", (createGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        "Mine craft",
        "Kids & Family",
        19.99M,
        new DateOnly(2011, 11, 18)
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetRouteName, new { Id = game.id }, game);
}
);

// PUT games/:id
app.MapPatch("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var gameIndex = games.FindIndex(x => x.id == id);
    if (gameIndex > -1)
    {
        games[gameIndex] = games[gameIndex] with
        {
            id = games[gameIndex].id,
            name = updatedGame.name ?? games[gameIndex].name,
            genre = updatedGame.genre ?? games[gameIndex].genre,
            releaseDate = updatedGame.releaseDate ?? games[gameIndex].releaseDate,
            price = updatedGame.price ?? games[gameIndex].price,
        };

        return Results.NoContent();
    }

    return Results.NotFound(new { error = "Invalid input" });
});

// DELETE - games/:id
app.MapDelete("games/{id}", (int id) =>
{
    int gameIndex = games.FindIndex(game => game.id == id);
    if (gameIndex > -1)
    {
        games.RemoveAt(gameIndex);
        return Results.NoContent();
    }
    return Results.NotFound(new { error = "Invalid id: " + id });
});
app.Run();

