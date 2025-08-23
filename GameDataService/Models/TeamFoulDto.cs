namespace GameDataService.Models;

public class TeamFoulDto
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int Period { get; set; }
    public int Count { get; set; }
}
