using Microsoft.EntityFrameworkCore;

namespace Rtl.Shows.Repository;

public class ShowsDbContext : DbContext
{
    public DbSet<Show> Shows { get; set; }
    public DbSet<Cast> Casts { get; set; }
    public DbSet<ShowCast> ShowCasts { get; set; }

    public ShowsDbContext(DbContextOptions<ShowsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShowCast>()
            .HasKey(bc => new { bc.ShowId, PersonId = bc.CastId });

        modelBuilder.Entity<ShowCast>()
            .HasOne(bc => bc.Show)
            .WithMany(b => b.ShowCasts)
            .HasForeignKey(bc => bc.ShowId);

        modelBuilder.Entity<ShowCast>()
            .HasOne(bc => bc.Cast)
            .WithMany(c => c.ShowCasts)
            .HasForeignKey(bc => bc.CastId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            var entity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entity.LastUpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}