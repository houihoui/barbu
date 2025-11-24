namespace Barbu.Domain.Enums;

/// <summary>
/// Types de contrats disponibles dans le jeu du Barbu
/// </summary>
public enum ContractType
{
    /// <summary>
    /// Barbu : éviter de prendre le Roi de Cœur (-20 points)
    /// </summary>
    Barbu = 1,

    /// <summary>
    /// Pas de plis : chaque pli pris = -2 points
    /// </summary>
    NoPlis = 2,

    /// <summary>
    /// Cœurs : chaque cœur pris = -2 points, As de cœur = -6 points
    /// </summary>
    Coeurs = 3,

    /// <summary>
    /// Dames : chaque Dame prise = -6 points
    /// </summary>
    Dames = 4,

    /// <summary>
    /// 2 derniers plis : avant-dernier pli = -10, dernier pli = -20
    /// </summary>
    DeuxDerniersPlis = 5,

    /// <summary>
    /// Atout : chaque pli = +5 points (contrat positif)
    /// Seuls les flancs peuvent contrer le déclarant
    /// </summary>
    Atout = 6,

    /// <summary>
    /// Réussite : 1er = +45, 2e = +20, 3e = +10, 4e = -10
    /// Seuls les flancs peuvent contrer le déclarant
    /// </summary>
    Reussite = 7
}
