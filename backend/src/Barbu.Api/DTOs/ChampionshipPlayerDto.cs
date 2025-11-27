namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour un joueur dans un championnat
/// </summary>
public class ChampionshipPlayerDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string? PlayerAvatar { get; set; }
    public decimal TotalPoints { get; set; }
    public int GamesPlayed { get; set; }
    public int Ranking { get; set; }
}
