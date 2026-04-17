using System.Text.Json.Serialization;
using ErrorOr;
using Mappy.Domain.Common;

namespace Mappy.Domain.Stops;

public class Stop: AuditableEntity 
{
    public string Name { get; private set; }
    public Uri? ImageUri { get; private set; }

    public Guid? ItineraryId { get; private set; }
    
    public Stop(string name, Uri? imageUri, Guid? id = null): base(id)
    {
        Name = name;
        ImageUri = imageUri;
    }

    public ErrorOr<Success> SetItineraryId(Guid itineraryId)
    {
        if (ItineraryId is not null)
            return StopErrors.ItineraryAlreadySet;
        
        ItineraryId = itineraryId;

        return Result.Success;
    }
    
    [JsonConstructor]
    public Stop(Guid id, string name, Uri? imageUri, Guid? itineraryId): base(id)
    {
        Name = name;
        ImageUri = imageUri;
        ItineraryId = itineraryId;
    }
    
    private Stop() { }
}

public static class StopErrors
{
    public static Error ItineraryAlreadySet => Error.Conflict(code: "SetItinerary", description: "Itinerary already set");
}