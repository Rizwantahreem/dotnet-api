using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateGameDto(
    [StringLength(50)] string? name,
    [StringLength(50)] string? genre,
    [Range(1, 100)] decimal? price,
    DateOnly? releaseDate
);
