namespace Barbu.Domain.Entities;

/// <summary>
/// Représente un joueur
/// </summary>
public class Player
{
    /// <summary>
    /// Identifiant unique du joueur
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du joueur
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// URL ou chemin de l'avatar
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Nombre total de parties jouées
    /// </summary>
    public int GamesPlayed { get; set; }

    /// <summary>
    /// Nombre de victoires
    /// </summary>
    public int Wins { get; set; }

    /// <summary>
    /// Date de création du joueur
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date de dernière modification
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Navigation : parties auxquelles le joueur participe
    /// </summary>
    public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    /// <summary>
    /// Navigation : championnats auxquels le joueur participe
    /// </summary>
    public ICollection<ChampionshipPlayer> ChampionshipPlayers { get; set; } = new List<ChampionshipPlayer>();
}
