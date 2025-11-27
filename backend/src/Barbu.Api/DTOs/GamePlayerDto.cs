namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour un joueur dans une partie
/// </summary>
public class GamePlayerDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string? PlayerAvatar { get; set; }
    public int Position { get; set; }
    public int TotalScore { get; set; }
    public int RemainingChallenges { get; set; }
    public int RemainingSurcontres { get; set; }
}
