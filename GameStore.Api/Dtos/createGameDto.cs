using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class createGameDto(
    [Required][StringLength(50)] string name ,
    [Required][StringLength(50)] string genre,
    [Required][Range(1, 100)] decimal price,
    [Required] DateOnly releaseDate
);
