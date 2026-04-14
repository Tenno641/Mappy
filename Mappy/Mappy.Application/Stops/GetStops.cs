using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Stops;
using MediatR;

namespace Mappy.Application.Stops;

public record GetStopsQuery(Guid ItineraryId): IRequest<List<Stop>>;

public class GetStops : IRequestHandler<GetStopsQuery, List<Stop>>
{
    private readonly IStopsRepository _stopsRepository;
    
    public GetStops(IStopsRepository stopsRepository)
    {
        _stopsRepository = stopsRepository;
    }

    public async Task<List<Stop>> Handle(GetStopsQuery request, CancellationToken cancellationToken)
    {
        return await _stopsRepository.GetStopsByItineraryId(request.ItineraryId);
    }
}