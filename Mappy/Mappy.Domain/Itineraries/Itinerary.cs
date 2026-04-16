using ErrorOr;
using Mappy.Domain.Common;
using Mappy.Domain.Itineraries.Events;
using Mappy.Domain.Stops;

namespace Mappy.Domain.Itineraries;

public class Itinerary: AuditableEntity
{
    private readonly List<Guid> _stopsIds = [];
   
    public Guid UserId { get; }
    public string Name { get; }
    public string? Description { get; }

    public Itinerary(string name, string? description, Guid userId, Guid? id = null): base(id)
    {
        Name = name;
        Description = description;
        UserId = userId;
    }

    public ErrorOr<Success> AddStop(Stop stop)
    {
        if (_stopsIds.Contains(stop.Id))
            return ItineraryErrors.StopAlreadyExist;
        
        DomainEvents.Add(new StopAddedEvent(Id, stop));
        
        _stopsIds.Add(stop.Id);

        return Result.Success;
    }
    
    private Itinerary() { }
}

public static class ItineraryErrors
{
    public static Error StopAlreadyExist => Error.Conflict(code: "Itinerary.AddStop", description: "Stop already exists");
}