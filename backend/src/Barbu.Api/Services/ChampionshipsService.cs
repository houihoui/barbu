using Barbu.Api.DTOs;
using Barbu.Domain.Entities;
using Barbu.Domain.Enums;
using Barbu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbu.Api.Services;

/// <summary>
/// Service de gestion des championnats
/// </summary>
public class ChampionshipsService : IChampionshipsService
{
    private readonly BarbuDbContext _context;

    public ChampionshipsService(BarbuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ChampionshipDto>> GetAllChampionshipsAsync()
    {
        var championships = await _context.Championships
            .Include(c => c.ChampionshipPlayers)
                .ThenInclude(cp => cp.Player)
            .Include(c => c.Games)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return championships.Select(MapToDto);
    }

    public async Task<ChampionshipDto?> GetChampionshipByIdAsync(Guid id)
    {
        var championship = await _context.Championships
            .Include(c => c.ChampionshipPlayers)
                .ThenInclude(cp => cp.Player)
            .Include(c => c.Games)
            .FirstOrDefaultAsync(c => c.Id == id);

        return championship == null ? null : MapToDto(championship);
    }

    public async Task<ChampionshipDto> CreateChampionshipAsync(CreateChampionshipDto createChampionshipDto)
    {
        // Valider que tous les joueurs existent
        var players = await _context.Players
            .Where(p => createChampionshipDto.PlayerIds.Contains(p.Id))
            .ToListAsync();

        if (players.Count != createChampionshipDto.PlayerIds.Count)
        {
            throw new InvalidOperationException("Un ou plusieurs joueurs n'existent pas");
        }

        var championship = new Championship
        {
            Id = Guid.NewGuid(),
            Name = createChampionshipDto.Name,
            Description = createChampionshipDto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            StartDate = DateTime.UtcNow
        };

        _context.Championships.Add(championship);

        // Créer les ChampionshipPlayers
        foreach (var playerId in createChampionshipDto.PlayerIds)
        {
            var championshipPlayer = new ChampionshipPlayer
            {
                Id = Guid.NewGuid(),
                ChampionshipId = championship.Id,
                PlayerId = playerId,
                TotalPoints = 0,
                GamesPlayed = 0
            };
            _context.ChampionshipPlayers.Add(championshipPlayer);
        }

        await _context.SaveChangesAsync();

        // Recharger avec les relations
        return (await GetChampionshipByIdAsync(championship.Id))!;
    }

    public async Task<ChampionshipDto?> UpdateChampionshipAsync(Guid id, UpdateChampionshipDto updateChampionshipDto)
    {
        var championship = await _context.Championships.FindAsync(id);
        if (championship == null)
            return null;

        championship.Name = updateChampionshipDto.Name;
        championship.Description = updateChampionshipDto.Description;
        championship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetChampionshipByIdAsync(id);
    }

    public async Task<bool> StartChampionshipAsync(Guid id)
    {
        var championship = await _context.Championships.FindAsync(id);
        if (championship == null)
            return false;

        if (championship.EndDate.HasValue)
        {
            throw new InvalidOperationException("Le championnat est déjà terminé");
        }

        championship.StartDate = DateTime.UtcNow;
        championship.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CompleteChampionshipAsync(Guid id)
    {
        var championship = await _context.Championships.FindAsync(id);
        if (championship == null)
            return false;

        if (championship.EndDate.HasValue)
        {
            throw new InvalidOperationException("Le championnat est déjà terminé");
        }

        championship.IsActive = false;
        championship.EndDate = DateTime.UtcNow;
        championship.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddPlayerAsync(Guid championshipId, Guid playerId)
    {
        var championship = await _context.Championships
            .Include(c => c.ChampionshipPlayers)
            .FirstOrDefaultAsync(c => c.Id == championshipId);

        if (championship == null)
            return false;

        var player = await _context.Players.FindAsync(playerId);
        if (player == null)
        {
            throw new InvalidOperationException("Le joueur n'existe pas");
        }

        if (championship.ChampionshipPlayers.Any(cp => cp.PlayerId == playerId))
        {
            throw new InvalidOperationException("Le joueur fait déjà partie du championnat");
        }

        // Vérifier si le championnat a déjà des parties commencées
        var hasStartedGames = await _context.Games
            .AnyAsync(g => g.ChampionshipId == championshipId && g.Status != GameStatus.Pending);

        if (hasStartedGames)
        {
            throw new InvalidOperationException("Impossible d'ajouter un joueur à un championnat ayant des parties en cours");
        }

        var championshipPlayer = new ChampionshipPlayer
        {
            Id = Guid.NewGuid(),
            ChampionshipId = championshipId,
            PlayerId = playerId,
            TotalPoints = 0,
            GamesPlayed = 0
        };

        _context.ChampionshipPlayers.Add(championshipPlayer);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemovePlayerAsync(Guid championshipId, Guid playerId)
    {
        var championshipPlayer = await _context.ChampionshipPlayers
            .Include(cp => cp.Championship)
            .FirstOrDefaultAsync(cp => cp.ChampionshipId == championshipId && cp.PlayerId == playerId);

        if (championshipPlayer == null)
            return false;

        // Vérifier si le championnat a déjà des parties commencées
        var hasStartedGames = await _context.Games
            .AnyAsync(g => g.ChampionshipId == championshipId && g.Status != GameStatus.Pending);

        if (hasStartedGames)
        {
            throw new InvalidOperationException("Impossible de retirer un joueur d'un championnat ayant des parties en cours");
        }

        _context.ChampionshipPlayers.Remove(championshipPlayer);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteChampionshipAsync(Guid id)
    {
        var championship = await _context.Championships
            .Include(c => c.Games)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (championship == null)
            return false;

        if (championship.Games.Any())
        {
            throw new InvalidOperationException("Impossible de supprimer un championnat ayant des parties associées");
        }

        _context.Championships.Remove(championship);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ChampionshipDto MapToDto(Championship championship)
    {
        var players = championship.ChampionshipPlayers
            .OrderByDescending(cp => cp.TotalPoints)
            .Select((cp, index) => new ChampionshipPlayerDto
            {
                Id = cp.Id,
                PlayerId = cp.PlayerId,
                PlayerName = cp.Player.Name,
                PlayerAvatar = cp.Player.Avatar,
                TotalPoints = cp.TotalPoints,
                GamesPlayed = cp.GamesPlayed,
                Ranking = index + 1
            })
            .ToList();

        return new ChampionshipDto
        {
            Id = championship.Id,
            Name = championship.Name,
            Description = championship.Description,
            IsActive = championship.IsActive,
            CreatedAt = championship.CreatedAt,
            StartedAt = championship.StartDate,
            CompletedAt = championship.EndDate,
            Players = players,
            TotalGames = championship.Games?.Count ?? 0
        };
    }
}
