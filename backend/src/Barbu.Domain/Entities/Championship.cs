namespace Barbu.Domain.Entities;

/// <summary>
/// Représente un championnat (groupe de joueurs + plusieurs parties)
/// </summary>
public class Championship
{
    /// <summary>
    /// Identifiant unique du championnat
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du championnat
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description (optionnelle)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date de début du championnat
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date de fin du championnat (optionnelle)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Indique si le championnat est actif
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date de création
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date de dernière modification
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Navigation : joueurs participant au championnat
    /// </summary>
    public ICollection<ChampionshipPlayer> ChampionshipPlayers { get; set; } = new List<ChampionshipPlayer>();

    /// <summary>
    /// Navigation : parties du championnat
    /// </summary>
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
