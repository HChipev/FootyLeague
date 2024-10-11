using Data.Models;
using FootyLeague.Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class MainContext(DbContextOptions<MainContext> options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<Team> Teams { get; set; }

    public DbSet<Match> Matches { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetBaseEntityProperties();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Match>()
            .HasOne(x => x.HomeTeam)
            .WithMany(x => x.HomeMatches)
            .HasForeignKey(x => x.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Match>()
            .HasOne(x => x.AwayTeam)
            .WithMany(x => x.AwayMatches)
            .HasForeignKey(x => x.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void SetBaseEntityProperties()
    {
        var changedEntries = ChangeTracker
            .Entries()
            .Where(e =>
                e is { Entity: IBaseEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in changedEntries)
        {
            var entity = (IBaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedDateTime = DateTime.UtcNow;
            }
        }
    }
}
