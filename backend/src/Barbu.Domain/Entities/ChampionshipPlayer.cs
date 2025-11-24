namespace Barbu.Domain.Entities;

/// <summary>
/// Table de jonction entre Championship et Player avec points de championnat
/// </summary>
public class ChampionshipPlayer
{
    /// <summary>
    /// Identifiant unique
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID du championnat
    /// </summary>
    public Guid ChampionshipId { get; set; }

    /// <summary>
    /// ID du joueur
    /// </summary>
    public Guid PlayerId { get; set; }

    /// <summary>
    /// Points totaux accumulés dans le championnat
    /// Calculés selon le barème 3 ou 4 joueurs
    /// </summary>
    public decimal TotalPoints { get; set; }

    /// <summary>
    /// Nombre de parties jouées dans ce championnat
    /// </summary>
    public int GamesPlayed { get; set; }

    /// <summary>
    /// Date d'inscription au championnat
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// Navigation : championnat
    /// </summary>
    public Championship Championship { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur
    /// </summary>
    public Player Player { get; set; } = null!;
}
