using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;

namespace Mappy.Application.Common.Interfaces;

public interface IItinerariesRepository
{
    Task<List<Itinerary>> ListItinerariesAsync(string? search);
    Task<Itinerary?> GetByIdAsync(Guid itineraryId);
    Task UpdateAsync(Itinerary itinerary);
    Task AddAsync(Itinerary itinerary);
}