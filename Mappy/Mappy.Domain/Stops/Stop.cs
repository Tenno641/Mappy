using ErrorOr;
using Mappy.Domain.Common;

namespace Mappy.Domain.Stops;

public class Stop: AuditableEntity 
{
    private string _name;
    private Uri? _imageUri;

    public Guid? ItineraryId { get; private set; }
    
    public Stop(string name, Uri? imageUri, Guid? id = null): base(id)
    {
        _name = name;
        _imageUri = imageUri;
    }

    public ErrorOr<Success> SetItineraryId(Guid itineraryId)
    {
        if (ItineraryId is not null)
            return StopErrors.ItineraryAlreadySet;
        
        ItineraryId = itineraryId;

        return Result.Success;
    }
    
    private Stop() { }
}

public static class StopErrors
{
    public static Error ItineraryAlreadySet => Error.Conflict(code: "SetItinerary", description: "Itinerary already set");
}