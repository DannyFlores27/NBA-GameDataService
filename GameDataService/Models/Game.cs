using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.Models;

public class Game
{
    public int GameId { get; set; }
    public DateTime GameDate { get; set; }

    [ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }

    [ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }

    public int HomeScore { get; set; } = 0;
    public int AwayScore { get; set; } = 0;
    public int CurrentPeriod { get; set; } = 1;

    /// <summary>Tiempo restante del período en segundos.</summary>
    public int? RemainingTime { get; set; }

    public DateTime? PeriodStartTime { get; set; }

    public GameStatus GameStatus { get; set; } = GameStatus.NOT_STARTED;

    public Team? HomeTeam { get; set; }
    public Team? AwayTeam { get; set; }

    public ICollection<TeamFoul> TeamFouls { get; set; } = new List<TeamFoul>();
    public ICollection<PlayerFoul> PlayerFouls { get; set; } = new List<PlayerFoul>();
}
