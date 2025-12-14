using Barbu.Api.DTOs;
using Barbu.Domain.Entities;
using Barbu.Domain.Enums;
using Barbu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbu.Api.Services;

/// <summary>
/// Service de gestion des parties
/// </summary>
public class GamesService : IGamesService
{
    private readonly BarbuDbContext _context;

    public GamesService(BarbuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
    {
        var games = await _context.Games
            .Include(g => g.GamePlayers)
                .ThenInclude(gp => gp.Player)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        return games.Select(MapToDto);
    }

    public async Task<GameDto?> GetGameByIdAsync(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.GamePlayers)
                .ThenInclude(gp => gp.Player)
            .FirstOrDefaultAsync(g => g.Id == id);

        return game == null ? null : MapToDto(game);
    }

    public async Task<GameDto> CreateGameAsync(CreateGameDto createGameDto)
    {
        // Valider que le nombre de joueurs correspond à la liste
        if (createGameDto.PlayerIds.Count != createGameDto.PlayerCount)
        {
            throw new InvalidOperationException($"Le nombre de joueurs ({createGameDto.PlayerIds.Count}) ne correspond pas au PlayerCount ({createGameDto.PlayerCount})");
        }

        // Valider que tous les joueurs existent
        var players = await _context.Players
            .Where(p => createGameDto.PlayerIds.Contains(p.Id))
            .ToListAsync();

        if (players.Count != createGameDto.PlayerIds.Count)
        {
            throw new InvalidOperationException("Un ou plusieurs joueurs n'existent pas");
        }

        // Valider que le championnat existe si spécifié
        if (createGameDto.ChampionshipId.HasValue)
        {
            var championship = await _context.Championships.FindAsync(createGameDto.ChampionshipId.Value);
            if (championship == null)
            {
                throw new InvalidOperationException("Le championnat spécifié n'existe pas");
            }
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = createGameDto.Name,
            PlayerCount = createGameDto.PlayerCount,
            Status = GameStatus.Pending,
            CurrentDealNumber = 0,
            ChampionshipId = createGameDto.ChampionshipId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            StartedAt = DateTime.UtcNow
        };

        _context.Games.Add(game);

        // Créer les GamePlayers
        for (int i = 0; i < createGameDto.PlayerIds.Count; i++)
        {
            var gamePlayer = new GamePlayer
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                PlayerId = createGameDto.PlayerIds[i],
                Position = i + 1,
                TotalScore = 0,
                RemainingChallenges = 2,
                RemainingSurcontres = 2
            };
            _context.GamePlayers.Add(gamePlayer);
        }

        await _context.SaveChangesAsync();

        // Recharger avec les relations
        return (await GetGameByIdAsync(game.Id))!;
    }

    public async Task<bool> StartGameAsync(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
            return false;

        if (game.Status != GameStatus.Pending)
        {
            throw new InvalidOperationException("La partie a déjà été démarrée");
        }

        game.Status = GameStatus.InProgress;
        game.StartedAt = DateTime.UtcNow;
        game.UpdatedAt = DateTime.UtcNow;
        game.CurrentDealNumber = 1;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteGameAsync(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.GamePlayers)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return false;

        if (game.Status != GameStatus.InProgress)
        {
            throw new InvalidOperationException("La partie n'est pas en cours");
        }

        game.Status = GameStatus.Completed;
        game.CompletedAt = DateTime.UtcNow;
        game.UpdatedAt = DateTime.UtcNow;

        // Mettre à jour les statistiques des joueurs
        foreach (var gamePlayer in game.GamePlayers)
        {
            var player = await _context.Players.FindAsync(gamePlayer.PlayerId);
            if (player != null)
            {
                player.GamesPlayed++;
                player.UpdatedAt = DateTime.UtcNow;
            }
        }

        // Déterminer le gagnant (score le plus bas)
        var winner = game.GamePlayers.OrderBy(gp => gp.TotalScore).First();
        var winnerPlayer = await _context.Players.FindAsync(winner.PlayerId);
        if (winnerPlayer != null)
        {
            winnerPlayer.Wins++;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteGameAsync(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
            return false;

        if (game.Status == GameStatus.InProgress)
        {
            throw new InvalidOperationException("Impossible de supprimer une partie en cours");
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<GameDetailsDto?> GetGameDetailsAsync(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.GamePlayers)
                .ThenInclude(gp => gp.Player)
            .Include(g => g.Deals)
                .ThenInclude(d => d.Declarer)
                    .ThenInclude(gp => gp.Player)
            .Include(g => g.Deals)
                .ThenInclude(d => d.DealScores)
                    .ThenInclude(ds => ds.GamePlayer)
                        .ThenInclude(gp => gp.Player)
            .Include(g => g.Deals)
                .ThenInclude(d => d.Challenges)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return null;

        return new GameDetailsDto
        {
            Id = game.Id,
            Name = game.Name,
            PlayerCount = game.PlayerCount,
            Status = game.Status,
            CurrentDealNumber = game.CurrentDealNumber,
            ChampionshipId = game.ChampionshipId,
            CreatedAt = game.CreatedAt,
            StartedAt = game.StartedAt,
            CompletedAt = game.CompletedAt,
            Players = game.GamePlayers
                .OrderBy(gp => gp.Position)
                .Select(gp => new GamePlayerDto
                {
                    Id = gp.Id,
                    PlayerId = gp.PlayerId,
                    PlayerName = gp.Player.Name,
                    PlayerAvatar = gp.Player.Avatar,
                    Position = gp.Position,
                    TotalScore = gp.TotalScore,
                    RemainingChallenges = gp.RemainingChallenges,
                    RemainingSurcontres = gp.RemainingSurcontres
                })
                .ToList(),
            Deals = game.Deals
                .OrderBy(d => d.DealNumber)
                .Select(MapDealToDto)
                .ToList()
        };
    }

    public async Task<IEnumerable<DealDto>> GetGameDealsAsync(Guid id)
    {
        var deals = await _context.Deals
            .Include(d => d.Declarer)
                .ThenInclude(gp => gp.Player)
            .Include(d => d.DealScores)
                .ThenInclude(ds => ds.GamePlayer)
                    .ThenInclude(gp => gp.Player)
            .Include(d => d.Challenges)
            .Where(d => d.GameId == id)
            .OrderBy(d => d.DealNumber)
            .ToListAsync();

        return deals.Select(MapDealToDto);
    }

    private static GameDto MapToDto(Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            PlayerCount = game.PlayerCount,
            Status = game.Status,
            CurrentDealNumber = game.CurrentDealNumber,
            ChampionshipId = game.ChampionshipId,
            CreatedAt = game.CreatedAt,
            StartedAt = game.StartedAt,
            CompletedAt = game.CompletedAt,
            Players = game.GamePlayers
                .OrderBy(gp => gp.Position)
                .Select(gp => new GamePlayerDto
                {
                    Id = gp.Id,
                    PlayerId = gp.PlayerId,
                    PlayerName = gp.Player.Name,
                    PlayerAvatar = gp.Player.Avatar,
                    Position = gp.Position,
                    TotalScore = gp.TotalScore,
                    RemainingChallenges = gp.RemainingChallenges,
                    RemainingSurcontres = gp.RemainingSurcontres
                })
                .ToList()
        };
    }

    private static DealDto MapDealToDto(Deal deal)
    {
        return new DealDto
        {
            Id = deal.Id,
            GameId = deal.GameId,
            DealNumber = deal.DealNumber,
            DeclarerGamePlayerId = deal.DeclarerGamePlayerId,
            DeclarerPlayerName = deal.Declarer.Player.Name,
            ContractType = deal.ContractType,
            IsCompleted = deal.IsCompleted,
            StartedAt = deal.StartedAt,
            CompletedAt = deal.CompletedAt,
            Scores = deal.DealScores.Select(MapDealScoreToDto).ToList(),
            Challenges = deal.Challenges.Select(MapChallengeToDto).ToList()
        };
    }

    private static DealScoreDto MapDealScoreToDto(DealScore score)
    {
        return new DealScoreDto
        {
            Id = score.Id,
            DealId = score.DealId,
            GamePlayerId = score.GamePlayerId,
            PlayerName = score.GamePlayer.Player.Name,
            BaseScore = score.BaseScore,
            FinalScore = score.FinalScore,
            ScoreDetails = score.ScoreDetails
        };
    }

    private static ChallengeDto MapChallengeToDto(Challenge challenge)
    {
        return new ChallengeDto
        {
            Id = challenge.Id,
            DealId = challenge.DealId,
            Type = challenge.Type,
            ChallengerGamePlayerId = challenge.ChallengerGamePlayerId,
            ChallengerPlayerName = challenge.Challenger.Player.Name,
            ChallengedGamePlayerId = challenge.ChallengedGamePlayerId,
            ChallengedPlayerName = challenge.Challenged.Player.Name,
            ScoreDifference = challenge.ScoreDifference,
            PointsTransferred = challenge.PointsTransferred
        };
    }
}
