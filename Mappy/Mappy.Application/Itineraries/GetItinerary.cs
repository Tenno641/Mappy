using Mappy.Domain.Itineraries;
using MediatR;
using ErrorOr;
using Mappy.Application.Common.Interfaces;

namespace Mappy.Application.Itineraries;

public record GetItineraryQuery(Guid ItineraryId) : IRequest<ErrorOr<Itinerary>>;

public class GetItinerary: IRequestHandler<GetItineraryQuery, ErrorOr<Itinerary>>
{
    private readonly IItinerariesRepository _itinerariesRepository;
    
    public GetItinerary(IItinerariesRepository itinerariesRepository)
    {
        _itinerariesRepository = itinerariesRepository;
    }

    public async Task<ErrorOr<Itinerary>> Handle(GetItineraryQuery query, CancellationToken cancellationToken)
    {
        Itinerary? itinerary = await _itinerariesRepository.GetByIdAsync(query.ItineraryId);

        if (itinerary is null)
            return Error.NotFound(description: "Itinerary is not found");

        return itinerary;
    }
}