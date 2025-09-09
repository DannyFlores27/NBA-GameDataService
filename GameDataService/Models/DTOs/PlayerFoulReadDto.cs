namespace GameDataService.Models.DTOs;

public class PlayerFoulReadDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int GameId { get; set; }
    public int Period { get; set; }
    public int FoulCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public PlayerReadDto? Player { get; set; }
    public GameReadDto? Game { get; set; }
}
