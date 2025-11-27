using Barbu.Api.DTOs;

namespace Barbu.Api.Services;

/// <summary>
/// Interface du service de gestion des championnats
/// </summary>
public interface IChampionshipsService
{
    Task<IEnumerable<ChampionshipDto>> GetAllChampionshipsAsync();
    Task<ChampionshipDto?> GetChampionshipByIdAsync(Guid id);
    Task<ChampionshipDto> CreateChampionshipAsync(CreateChampionshipDto createChampionshipDto);
    Task<ChampionshipDto?> UpdateChampionshipAsync(Guid id, UpdateChampionshipDto updateChampionshipDto);
    Task<bool> StartChampionshipAsync(Guid id);
    Task<bool> CompleteChampionshipAsync(Guid id);
    Task<bool> AddPlayerAsync(Guid championshipId, Guid playerId);
    Task<bool> RemovePlayerAsync(Guid championshipId, Guid playerId);
    Task<bool> DeleteChampionshipAsync(Guid id);
}
