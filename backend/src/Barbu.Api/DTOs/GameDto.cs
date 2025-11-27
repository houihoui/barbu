using Barbu.Domain.Enums;

namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la lecture d'une partie
/// </summary>
public class GameDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int PlayerCount { get; set; }
    public GameStatus Status { get; set; }
    public int CurrentDealNumber { get; set; }
    public Guid? ChampionshipId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<GamePlayerDto> Players { get; set; } = new();
}
