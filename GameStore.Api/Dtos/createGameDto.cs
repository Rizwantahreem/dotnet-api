namespace GameStore.Api.Dtos;

public record class createGameDto(
    string name,
    string genre,
    decimal price,
    DateOnly releaseDate
);
