using Microsoft.EntityFrameworkCore;

namespace Barbu.Infrastructure.Data;

public class BarbuDbContext : DbContext
{
    public BarbuDbContext(DbContextOptions<BarbuDbContext> options)
        : base(options)
    {
    }

    // DbSets seront ajoutés ici au fur et à mesure
    // public DbSet<Player> Players => Set<Player>();
    // public DbSet<Game> Games => Set<Game>();
    // etc.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Les configurations seront ajoutées ici
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarbuDbContext).Assembly);
    }
}
