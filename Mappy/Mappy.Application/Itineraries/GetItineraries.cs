using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;
using MediatR;

namespace Mappy.Application.Itineraries;

public record GetItineraries(string? Search): IRequest<List<Itinerary>>;

public class GetItinerariesQueryHandler : IRequestHandler<GetItineraries, List<Itinerary>>
{
    private readonly IItinerariesRepository _itinerariesRepository;
    
    public GetItinerariesQueryHandler(IItinerariesRepository itinerariesRepository)
    {
        _itinerariesRepository = itinerariesRepository;
    }

    public async Task<List<Itinerary>> Handle(GetItineraries request, CancellationToken cancellationToken)
    {
        return await _itinerariesRepository.ListItinerariesAsync(request.Search);
    }
}