using System.ComponentModel.DataAnnotations;

namespace GameDataService.Models.DTOs;

public class TeamWriteDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string City { get; set; } = string.Empty;
}
