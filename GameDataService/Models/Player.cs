using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.Models;

public class Player
{
    public int PlayerId { get; set; }

    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }

    [Required]
    public int JerseyNumber { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;

    [MaxLength(50)]
    public string? Position { get; set; }

    public Team? Team { get; set; }
}
