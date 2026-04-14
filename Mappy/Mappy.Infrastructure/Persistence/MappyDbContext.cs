using System.Reflection;
using Mappy.Domain.Common;
using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mappy.Infrastructure.Persistence;

public class MappyDbContext: DbContext
{
    public MappyDbContext(DbContextOptions<MappyDbContext> options) : base(options)
    {
    }
    
    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<Stop> Stops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (EntityEntry<AuditableEntity> entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = ""; // TODO: User Service
                entry.Entity.CreatedOn = DateTime.UtcNow;
                entry.Entity.ModifiedBy = ""; // TODO: UserService
                entry.Entity.ModifiedOn = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = ""; // TODO: UserService
                entry.Entity.ModifiedOn = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}