using ErrorOr;
using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;
using MediatR;

namespace Mappy.Application.Stops;

public record GetStopQuery(Guid ItineraryId, Guid StopId) : IRequest<ErrorOr<Stop>>;

public class GetStop: IRequestHandler<GetStopQuery, ErrorOr<Stop>>
{
    private readonly IStopsRepository _stopsRepository;
    private readonly IItinerariesRepository _itinerariesRepository;
    
    public GetStop(IStopsRepository stopsRepository, IItinerariesRepository itinerariesRepository)
    {
        _stopsRepository = stopsRepository;
        _itinerariesRepository = itinerariesRepository;
    }
    
    public async Task<ErrorOr<Stop>> Handle(GetStopQuery request, CancellationToken cancellationToken)
    {
        Itinerary? itinerary = await _itinerariesRepository.GetByIdAsync(request.ItineraryId);
        if (itinerary is null)
            return Error.NotFound(description: "Itinerary is not found");
        
        Stop? stop = await _stopsRepository.GetStopAsync(request.StopId);
        if (stop is null)
            return Error.NotFound(description: "Stop is not found");

        return stop;
    }
}