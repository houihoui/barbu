namespace Barbu.Domain.Entities;

/// <summary>
/// Représente le score d'un joueur pour une donne spécifique
/// </summary>
public class DealScore
{
    /// <summary>
    /// Identifiant unique
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la donne
    /// </summary>
    public Guid DealId { get; set; }

    /// <summary>
    /// ID du GamePlayer (joueur dans cette partie)
    /// </summary>
    public Guid GamePlayerId { get; set; }

    /// <summary>
    /// Score de base (avant application des contres/surcontres)
    /// </summary>
    public int BaseScore { get; set; }

    /// <summary>
    /// Score après application des contres/surcontres
    /// </summary>
    public int FinalScore { get; set; }

    /// <summary>
    /// Détails du score (ex: "3 plis × 5 = 15 points" pour Atout)
    /// </summary>
    public string? ScoreDetails { get; set; }

    /// <summary>
    /// Navigation : donne
    /// </summary>
    public Deal Deal { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur (via GamePlayer)
    /// </summary>
    public GamePlayer GamePlayer { get; set; } = null!;
}
