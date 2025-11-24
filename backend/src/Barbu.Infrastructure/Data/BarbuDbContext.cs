using Barbu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Barbu.Infrastructure.Data;

public class BarbuDbContext : DbContext
{
    public BarbuDbContext(DbContextOptions<BarbuDbContext> options)
        : base(options)
    {
    }

    // DbSets des entités principales
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GamePlayer> GamePlayers => Set<GamePlayer>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<DealScore> DealScores => Set<DealScore>();
    public DbSet<Challenge> Challenges => Set<Challenge>();
    public DbSet<Championship> Championships => Set<Championship>();
    public DbSet<ChampionshipPlayer> ChampionshipPlayers => Set<ChampionshipPlayer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de l'entité Player
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.HasIndex(e => e.Name);
        });

        // Configuration de l'entité Game
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ChampionshipId);
        });

        // Configuration de l'entité GamePlayer
        modelBuilder.Entity<GamePlayer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Game)
                .WithMany(g => g.GamePlayers)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Player)
                .WithMany(p => p.GamePlayers)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.GameId, e.PlayerId }).IsUnique();
            entity.HasIndex(e => new { e.GameId, e.Position }).IsUnique();
        });

        // Configuration de l'entité Deal
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Game)
                .WithMany(g => g.Deals)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Declarer)
                .WithMany(gp => gp.DeclaredDeals)
                .HasForeignKey(e => e.DeclarerGamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.GameId, e.DealNumber }).IsUnique();
        });

        // Configuration de l'entité DealScore
        modelBuilder.Entity<DealScore>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Deal)
                .WithMany(d => d.DealScores)
                .HasForeignKey(e => e.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.GamePlayer)
                .WithMany()
                .HasForeignKey(e => e.GamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ScoreDetails).HasMaxLength(500);

            entity.HasIndex(e => new { e.DealId, e.GamePlayerId }).IsUnique();
        });

        // Configuration de l'entité Challenge
        modelBuilder.Entity<Challenge>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Deal)
                .WithMany(d => d.Challenges)
                .HasForeignKey(e => e.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Challenger)
                .WithMany()
                .HasForeignKey(e => e.ChallengerGamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Challenged)
                .WithMany()
                .HasForeignKey(e => e.ChallengedGamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.DealId, e.ChallengerGamePlayerId, e.ChallengedGamePlayerId }).IsUnique();
        });

        // Configuration de l'entité Championship
        modelBuilder.Entity<Championship>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
        });

        // Configuration de l'entité ChampionshipPlayer
        modelBuilder.Entity<ChampionshipPlayer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Championship)
                .WithMany(c => c.ChampionshipPlayers)
                .HasForeignKey(e => e.ChampionshipId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Player)
                .WithMany(p => p.ChampionshipPlayers)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.TotalPoints).HasPrecision(10, 1);

            entity.HasIndex(e => new { e.ChampionshipId, e.PlayerId }).IsUnique();
        });
    }
}
