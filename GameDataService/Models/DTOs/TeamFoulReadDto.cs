namespace GameDataService.Models.DTOs;

public class TeamFoulReadDto
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int GameId { get; set; }
    public int Period { get; set; }
    public int TotalFouls { get; set; }
    public DateTime CreatedAt { get; set; }
    public TeamReadDto? Team { get; set; }
    public GameReadDto? Game { get; set; }
}
