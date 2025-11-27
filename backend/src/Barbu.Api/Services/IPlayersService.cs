using Barbu.Api.DTOs;

namespace Barbu.Api.Services;

/// <summary>
/// Interface du service de gestion des joueurs
/// </summary>
public interface IPlayersService
{
    Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
    Task<PlayerDto?> GetPlayerByIdAsync(Guid id);
    Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto createPlayerDto);
    Task<PlayerDto?> UpdatePlayerAsync(Guid id, UpdatePlayerDto updatePlayerDto);
    Task<bool> DeletePlayerAsync(Guid id);
}
