using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models;

public class Team
{
    public int TeamId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? LogoUrl { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
}

