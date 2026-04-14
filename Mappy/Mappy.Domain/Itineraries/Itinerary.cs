using Mappy.Domain.Common;

namespace Mappy.Domain.Itineraries;

public class Itinerary: AuditableEntity
{
    private readonly List<Guid> _stopsIds = [];
    private Guid _userId;
   
    public string Name { get; }
    public string? Description { get; }

    public Itinerary(string name, string? description, Guid userId, Guid? id = null): base(id)
    {
        Name = name;
        Description = description;
        _userId = userId;
    }
    
    private Itinerary() { }
}