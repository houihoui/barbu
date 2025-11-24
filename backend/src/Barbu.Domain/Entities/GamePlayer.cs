namespace Barbu.Domain.Entities;

/// <summary>
/// Table de jonction entre Game et Player avec score total
/// </summary>
public class GamePlayer
{
    /// <summary>
    /// Identifiant unique
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la partie
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// ID du joueur
    /// </summary>
    public Guid PlayerId { get; set; }

    /// <summary>
    /// Position du joueur dans la partie (0 à 3)
    /// Utilisé pour déterminer l'ordre de jeu et le déclarant
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Score total accumulé dans cette partie
    /// </summary>
    public int TotalScore { get; set; }

    /// <summary>
    /// Nombre de contres restants pour ce joueur
    /// Initialisé à (nombre de joueurs - 1) × 7
    /// </summary>
    public int RemainingChallenges { get; set; }

    /// <summary>
    /// Nombre de surcontres restants pour ce joueur
    /// Limité aux contrats où il est déclarant (Atout et Réussite)
    /// </summary>
    public int RemainingSurcontres { get; set; }

    /// <summary>
    /// Navigation : partie
    /// </summary>
    public Game Game { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur
    /// </summary>
    public Player Player { get; set; } = null!;

    /// <summary>
    /// Navigation : contrats déclarés par ce joueur
    /// </summary>
    public ICollection<Deal> DeclaredDeals { get; set; } = new List<Deal>();
}
