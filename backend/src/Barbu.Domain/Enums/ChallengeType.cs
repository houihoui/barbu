namespace Barbu.Domain.Enums;

/// <summary>
/// Type de défi entre joueurs
/// </summary>
public enum ChallengeType
{
    /// <summary>
    /// Contre : transfert de l'écart entre 2 joueurs
    /// </summary>
    Contre = 1,

    /// <summary>
    /// Surcontre : transfert de 2× l'écart entre le déclarant et un joueur
    /// </summary>
    Surcontre = 2
}
