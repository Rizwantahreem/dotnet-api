using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapper;

public static class GameMapper
{
    public static Game ToEntity(this createGameDto gameDto)
    {
        return new Game()
        {
            Name = gameDto.name,
            GenreId = gameDto.genreId,
            Price = gameDto.price,
            ReleaseDate = gameDto.releaseDate,
        };
    }

    public static GameDto ToDto(this Game entity)
    {
        return new(
            entity.Id,
            entity.Name,
            entity.Genre!.Name,
            entity.Price,
            entity.ReleaseDate
        );
    }

    public static void ApplyUpdates(this UpdateGameDto gameDto, Game game)
    {
        game.Name = gameDto.name ?? game.Name;
        game.GenreId = gameDto.genreId ?? game.GenreId;
        game.Price = gameDto.price ?? game.Price;
        game.ReleaseDate = gameDto.releaseDate ?? game.ReleaseDate;
    }
}
