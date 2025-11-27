using Barbu.Api.DTOs;
using Barbu.Domain.Entities;
using Barbu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbu.Api.Services;

/// <summary>
/// Service de gestion des joueurs
/// </summary>
public class PlayersService : IPlayersService
{
    private readonly BarbuDbContext _context;

    public PlayersService(BarbuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
    {
        var players = await _context.Players
            .OrderBy(p => p.Name)
            .ToListAsync();

        return players.Select(MapToDto);
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(Guid id)
    {
        var player = await _context.Players.FindAsync(id);
        return player == null ? null : MapToDto(player);
    }

    public async Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto createPlayerDto)
    {
        var player = new Player
        {
            Id = Guid.NewGuid(),
            Name = createPlayerDto.Name,
            Avatar = createPlayerDto.Avatar,
            GamesPlayed = 0,
            Wins = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return MapToDto(player);
    }

    public async Task<PlayerDto?> UpdatePlayerAsync(Guid id, UpdatePlayerDto updatePlayerDto)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
            return null;

        player.Name = updatePlayerDto.Name;
        player.Avatar = updatePlayerDto.Avatar;
        player.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(player);
    }

    public async Task<bool> DeletePlayerAsync(Guid id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
            return false;

        // Vérifier si le joueur a des parties en cours
        var hasActiveGames = await _context.GamePlayers
            .AnyAsync(gp => gp.PlayerId == id && gp.Game.Status == Domain.Enums.GameStatus.InProgress);

        if (hasActiveGames)
            throw new InvalidOperationException("Impossible de supprimer un joueur ayant des parties en cours");

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return true;
    }

    private static PlayerDto MapToDto(Player player)
    {
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            Avatar = player.Avatar,
            GamesPlayed = player.GamesPlayed,
            Wins = player.Wins,
            CreatedAt = player.CreatedAt,
            UpdatedAt = player.UpdatedAt
        };
    }
}
