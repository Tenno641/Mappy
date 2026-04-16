using System.Reflection;
using Mappy.Domain.Common;
using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;
using Mappy.Infrastructure.Outbox;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mappy.Infrastructure.Persistence;

public class MappyDbContext: DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublisher _publisher;
        
    public const string DomainEventsKey = "DomainEvents";
    
    public MappyDbContext(DbContextOptions<MappyDbContext> options, IHttpContextAccessor contextAccessor, IPublisher publisher) : base(options)
    {
        _httpContextAccessor = contextAccessor;
        _publisher = publisher;
    }
    
    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
        
        var domainEvents = ChangeTracker.Entries<Entity>().SelectMany(e => e.Entity.PopDomainEvents()).ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        // if (IsRequestBeingProcessed)
        // {
        //     SetDomainEventsToBeProcessed(domainEvents);
        //     return await base.SaveChangesAsync(cancellationToken);
        // }
        //
        // await PublishDomainEvents(domainEvents);
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private bool IsRequestBeingProcessed => _httpContextAccessor.HttpContext is not null;

    private void SetDomainEventsToBeProcessed(List<IDomainEvent> events)
    {
        if (_httpContextAccessor.HttpContext.Items.TryGetValue(DomainEventsKey, out var domainEvents) 
            && domainEvents is Queue<IDomainEvent> existingDomainEvents)
        {
            events.ForEach(existingDomainEvents.Enqueue);
        }
        
        _httpContextAccessor.HttpContext.Items["DomainEvents"] = new Queue<IDomainEvent>(events);
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
            await _publisher.Publish(domainEvent);
    }
}