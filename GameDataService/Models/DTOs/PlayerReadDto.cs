namespace GameDataService.Models.DTOs;

public class PlayerReadDto
{
    public int PlayerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public string Position { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public TeamReadDto? Team { get; set; }
}
