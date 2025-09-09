namespace GameDataService.Models.DTOs;

public class TeamReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public List<PlayerReadDto> Players { get; set; } = new();
}
