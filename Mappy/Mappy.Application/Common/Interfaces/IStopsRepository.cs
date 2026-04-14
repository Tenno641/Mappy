using Mappy.Domain.Stops;

namespace Mappy.Application.Common.Interfaces;

public interface IStopsRepository
{
    Task<List<Stop>> GetStopsByItineraryId(Guid itineraryId);
}