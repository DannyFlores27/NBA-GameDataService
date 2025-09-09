using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class TeamFoulWriteDto
{
    [Required]
    public int TeamId { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    [Range(1, 4)]
    public int Period { get; set; }

    [Required]
    [Range(0, 10)]
    public int TotalFouls { get; set; }
}
