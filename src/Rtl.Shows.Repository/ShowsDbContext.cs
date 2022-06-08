using Microsoft.EntityFrameworkCore;

namespace Rtl.Shows.Repository;

public class ShowsDbContext : DbContext
{
    public DbSet<Show> Shows { get; set; }
    public DbSet<Person> Casts { get; set; }
    public DbSet<ShowPerson> ShowPersons { get; set; }

    public ShowsDbContext(DbContextOptions<ShowsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShowPerson>()
            .HasKey(bc => new { bc.ShowId, bc.PersonId });

        modelBuilder.Entity<ShowPerson>()
            .HasOne(bc => bc.Show)
            .WithMany(b => b.ShowPersons)
            .HasForeignKey(bc => bc.ShowId);

        modelBuilder.Entity<ShowPerson>()
            .HasOne(bc => bc.Person)
            .WithMany(c => c.ShowPersons)
            .HasForeignKey(bc => bc.PersonId);
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