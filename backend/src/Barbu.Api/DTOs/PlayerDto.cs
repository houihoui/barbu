namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la lecture d'un joueur
/// </summary>
public class PlayerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public int GamesPlayed { get; set; }
    public int Wins { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
