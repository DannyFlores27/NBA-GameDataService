using GameDataService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.Models;

public class TeamFoul
{
    public int TeamFoulId { get; set; }

    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }

    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }

    public int Period { get; set; }
    public int TotalFouls { get; set; } = 0;

    public Game? Game { get; set; }
    public Team? Team { get; set; }
}
