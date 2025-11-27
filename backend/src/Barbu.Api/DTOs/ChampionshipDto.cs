namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la lecture d'un championnat
/// </summary>
public class ChampionshipDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<ChampionshipPlayerDto> Players { get; set; } = new();
    public int TotalGames { get; set; }
}
