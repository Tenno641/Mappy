using Mappy.Domain.Itineraries;

namespace Mappy.Application.Common.Interfaces;

public interface IItinerariesRepository
{
    Task<List<Itinerary>> ListItinerariesAsync(string? search);
}