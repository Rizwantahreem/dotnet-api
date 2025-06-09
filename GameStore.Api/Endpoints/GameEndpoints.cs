using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class gameEndpoints
{
    const string GetRouteName = "GetGame";

    private static readonly List<GameDto> games = [
        new (1, "Street Figther II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
        new (2, "Finally Fantasy XIV", "Roleplaying", 59.9M, new DateOnly(2010, 9, 30)),
        new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
    ];

    public static RouteGroupBuilder mapGameEndpoints(this WebApplication app)
    {
        // to avoid the prefix in repritition
        RouteGroupBuilder group = app.MapGroup("games")
                                    .WithParameterValidation(); // this is supported by the minimalApis.Extension package 

        // note - In minimal APIs we need the endpoint filter to enable the validation we added with annotations
        
        
        // GET - /games
        group.MapGet("/", () => games);

        // GET  - /games/:id
        group.MapGet("/{id}", (int id) => games.Find(game => game.id == id))
            .WithName(GetRouteName);

        // POST - /games
        group.MapPost("", (createGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.name,
                newGame.genre,
                newGame.price,
                newGame.releaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetRouteName, new { Id = game.id }, game);
        }
        );

        // PATCH - games/:id
        group.MapPatch("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            // Check: All fields are null or empty — reject!
            bool allFieldsAreEmpty =
                string.IsNullOrWhiteSpace(updatedGame.name) &&
                string.IsNullOrWhiteSpace(updatedGame.genre) &&
                updatedGame.price == null &&
                updatedGame.releaseDate == null;

            if (allFieldsAreEmpty)
            {
                return Results.BadRequest("At least one field must be provided for update.");
            }

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
        group.MapDelete("/{id}", (int id) =>
        {
            int gameIndex = games.FindIndex(game => game.id == id);
            if (gameIndex > -1)
            {
                games.RemoveAt(gameIndex);
                return Results.NoContent();
            }
            return Results.NotFound(new { error = "Invalid id: " + id });
        });

        return group;
    }
}
