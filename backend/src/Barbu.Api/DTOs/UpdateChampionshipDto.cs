using System.ComponentModel.DataAnnotations;

namespace Barbu.Api.DTOs;

/// <summary>
/// DTO pour la modification d'un championnat
/// </summary>
public class UpdateChampionshipDto
{
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 200 caractères")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string? Description { get; set; }
}
