using Mappy.Domain.Stops;

namespace Mappy.Application.Common.Interfaces;

public interface IStopsRepository
{
    Task<List<Stop>> GetStopsByItineraryId(Guid itineraryId);
    Task CreateStopAsync(Stop stop);
    Task<Stop?> GetStopAsync(Guid stopId);
}