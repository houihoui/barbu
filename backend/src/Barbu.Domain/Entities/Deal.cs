using Barbu.Domain.Enums;

namespace Barbu.Domain.Entities;

/// <summary>
/// Représente une donne (un tour de jeu avec un contrat)
/// </summary>
public class Deal
{
    /// <summary>
    /// Identifiant unique de la donne
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la partie
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Numéro de la donne (1 à 21 ou 28)
    /// </summary>
    public int DealNumber { get; set; }

    /// <summary>
    /// ID du joueur déclarant (celui qui choisit le contrat)
    /// </summary>
    public Guid DeclarerGamePlayerId { get; set; }

    /// <summary>
    /// Type de contrat choisi
    /// </summary>
    public ContractType ContractType { get; set; }

    /// <summary>
    /// Indique si la donne est terminée
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Date de début de la donne
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Date de fin de la donne
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Navigation : partie
    /// </summary>
    public Game Game { get; set; } = null!;

    /// <summary>
    /// Navigation : joueur déclarant
    /// </summary>
    public GamePlayer Declarer { get; set; } = null!;

    /// <summary>
    /// Navigation : scores de chaque joueur pour cette donne
    /// </summary>
    public ICollection<DealScore> DealScores { get; set; } = new List<DealScore>();

    /// <summary>
    /// Navigation : contres et surcontres pour cette donne
    /// </summary>
    public ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();
}
