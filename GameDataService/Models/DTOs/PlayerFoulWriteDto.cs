using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class PlayerFoulWriteDto
{
    [Required]
    public int PlayerId { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    [Range(1, 4)]
    public int Period { get; set; }

    [Required]
    [Range(1, 6)]
    public int FoulCount { get; set; }
}
