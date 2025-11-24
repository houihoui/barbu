using Barbu.Domain.Enums;

namespace Barbu.Domain.Entities;

/// <summary>
/// Représente une partie de Barbu
/// </summary>
public class Game
{
    /// <summary>
    /// Identifiant unique de la partie
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom de la partie (optionnel)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Nombre de joueurs (3 ou 4)
    /// </summary>
    public int PlayerCount { get; set; }

    /// <summary>
    /// Statut de la partie
    /// </summary>
    public GameStatus Status { get; set; }

    /// <summary>
    /// Numéro de la donne en cours (1 à 21 ou 28)
    /// </summary>
    public int CurrentDealNumber { get; set; }

    /// <summary>
    /// Date de début de la partie
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Date de fin de la partie (null si en cours)
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Date de création
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date de dernière modification
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// ID du championnat auquel appartient cette partie (optionnel)
    /// </summary>
    public Guid? ChampionshipId { get; set; }

    /// <summary>
    /// Navigation : championnat
    /// </summary>
    public Championship? Championship { get; set; }

    /// <summary>
    /// Navigation : joueurs participant à cette partie
    /// </summary>
    public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    /// <summary>
    /// Navigation : donnes de cette partie
    /// </summary>
    public ICollection<Deal> Deals { get; set; } = new List<Deal>();
}
