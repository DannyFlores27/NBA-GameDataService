using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class PlayerWriteDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Range(0, 99)]
    public int JerseyNumber { get; set; }

    [Required]
    [StringLength(20)]
    public string Position { get; set; } = string.Empty;

    [Required]
    public int TeamId { get; set; }
}
