using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class GameReadDto
{
    public int GameId { get; set; }
    public DateTime GameDate { get; set; }
    public int HomeTeamId { get; set; }
    public TeamReadDto? HomeTeam { get; set; }
    public int AwayTeamId { get; set; }
    public TeamReadDto? AwayTeam { get; set; }
    public int PeriodSeconds { get; set; }
}
