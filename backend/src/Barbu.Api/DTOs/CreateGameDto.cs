using System.ComponentModel.DataAnnotations;

namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la création d'une partie
/// </summary>
public class CreateGameDto
{
    [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Le nombre de joueurs est obligatoire")]
    [Range(3, 4, ErrorMessage = "Une partie doit avoir 3 ou 4 joueurs")]
    public int PlayerCount { get; set; }

    [Required(ErrorMessage = "La liste des joueurs est obligatoire")]
    [MinLength(3, ErrorMessage = "Une partie doit avoir au moins 3 joueurs")]
    [MaxLength(4, ErrorMessage = "Une partie ne peut pas avoir plus de 4 joueurs")]
    public List<Guid> PlayerIds { get; set; } = new();

    public Guid? ChampionshipId { get; set; }
}
