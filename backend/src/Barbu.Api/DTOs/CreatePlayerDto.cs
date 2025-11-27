using System.ComponentModel.DataAnnotations;

namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la création d'un joueur
/// </summary>
public class CreatePlayerDto
{
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "L'URL de l'avatar ne peut pas dépasser 500 caractères")]
    public string? Avatar { get; set; }
}
