using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapper;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class gameEndpoints
{
    const string GetRouteName = "GetGame";

    // here this 'this' is for the complirer to recognized that it's an Extension Function
    public static RouteGroupBuilder mapGameEndpoints(this WebApplication app)
    {
        // to avoid the prefix in repritition
        RouteGroupBuilder group = app.MapGroup("games")
                                    .WithParameterValidation(); // this is supported by the minimalApis.Extension package [endPoint filter]

        // note - In minimal APIs we need the endpoint filter to enable the validation we added with annotations

        // GET - /games
        group.MapGet("/", (GameStoreContext dbContext) => 
                        dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => game.ToDto())
                            .AsNoTracking()
        );

        // GET  - /games/:id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games
                            .Include(game => game.Genre)
                            .FirstOrDefaultAsync(game => game.Id == id);

            if (game == null)
            {
                return Results.NotFound(new { error = "Invalid id " + id });
            }

            return Results.Json(game.ToDto());
        })
        .WithName(GetRouteName);

        // POST - /games
        group.MapPost("", (createGameDto newGame, GameStoreContext dbContext) =>
        {
            // creating an entity object
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genre.Find(newGame.genreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            // Transfrorming into DTO as entity shouldn't be shown to the user
            return Results.CreatedAtRoute(GetRouteName, new { game.Id }, game.ToDto());
        }
        );

        // PATCH - games/:id
        group.MapPatch("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            // Check: All fields are null or empty — reject!
            bool allFieldsAreEmpty =
                string.IsNullOrWhiteSpace(updatedGame.name) &&
                updatedGame.price == null &&
                updatedGame.releaseDate == null &&
                updatedGame.genreId == null;

            if (allFieldsAreEmpty)
            {
                return Results.BadRequest("At least one field must be provided for update.");
            }

            var game = await dbContext.Games
                            .Include(game => game.Genre)
                            .FirstOrDefaultAsync(game => game.Id == id);

            if (game is null)
            {
                return Results.NotFound(new { error = "Invalid ID " + id });
            }

            updatedGame.ApplyUpdates(game);
            game.Genre = dbContext.Genre.Find(updatedGame.genreId);
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE - games/:id
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            // batch delete
            // dbContext.Games
            //         .Where(game => game.Id == id)
            //         .ExecuteDelete();

            var game = dbContext.Games.Find(id);
            if (game is null)
            {
                return Results.NotFound(new { error = "Invalid id " + id });
            }
            dbContext.Games.Remove(game);
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        return group;
    }
}



// For update we could have done this 
// dbContext.Entry(existingGame)
// .CurrentValues
// .SetValues(dto.toUpdateDto())
