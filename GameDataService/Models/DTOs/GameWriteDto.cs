using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class GameWriteDto
{
    [Required]
    public int HomeTeamId { get; set; }

    [Required]
    public int AwayTeamId { get; set; }

    [Required]
    public DateTime GameDate { get; set; }

    public int? PeriodSeconds { get; set; }
}

public class TimeDto
{
    [Required]
    public int PeriodSeconds { get; set; }
}

public class PointsDto
{
    [Required]
    public int Points { get; set; }
}
