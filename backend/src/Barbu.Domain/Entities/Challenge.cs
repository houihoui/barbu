using Barbu.Domain.Enums;

namespace Barbu.Domain.Entities;

/// <summary>
/// Représente un contre ou surcontre entre deux joueurs pour une donne
/// </summary>
public class Challenge
{
    /// <summary>
    /// Identifiant unique
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la donne concernée
    /// </summary>
    public Guid DealId { get; set; }

    /// <summary>
    /// Type de défi (Contre ou Surcontre)
    /// </summary>
    public ChallengeType Type { get; set; }

    /// <summary>
    /// ID du joueur qui lance le défi
    /// </summary>
    public Guid ChallengerGamePlayerId { get; set; }

    /// <summary>
    /// ID du joueur qui est défié
    /// </summary>
    public Guid ChallengedGamePlayerId { get; set; }

    /// <summary>
    /// Écart de score entre les deux joueurs
    /// Calculé après la donne
    /// </summary>
    public int ScoreDifference { get; set; }

    /// <summary>
    /// Points transférés (= ScoreDifference pour Contre, = ScoreDifference × 2 pour Surcontre)
    /// </summary>
    public int PointsTransferred { get; set; }

    /// <summary>
    /// Navigation : donne
    /// </summary>
    public Deal Deal { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur qui lance le défi
    /// </summary>
    public GamePlayer Challenger { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur qui est défié
    /// </summary>
    public GamePlayer Challenged { get; set; } = null!;
}
