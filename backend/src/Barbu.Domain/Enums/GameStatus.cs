namespace Barbu.Domain.Enums;

/// <summary>
/// Statut d'une partie
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// Partie en attente de démarrage
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Partie en cours
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Partie terminée
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Partie abandonnée
    /// </summary>
    Abandoned = 3
}
