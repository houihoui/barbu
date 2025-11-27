using Barbu.Api.DTOs;

namespace Barbu.Api.Services;

/// <summary>
/// Interface du service de gestion des parties
/// </summary>
public interface IGamesService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync();
    Task<GameDto?> GetGameByIdAsync(Guid id);
    Task<GameDto> CreateGameAsync(CreateGameDto createGameDto);
    Task<bool> StartGameAsync(Guid id);
    Task<bool> CompleteGameAsync(Guid id);
    Task<bool> DeleteGameAsync(Guid id);
}
