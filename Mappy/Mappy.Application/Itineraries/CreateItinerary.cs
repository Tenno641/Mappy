using MediatR;
using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;

namespace Mappy.Application.Itineraries;

public record CreateItineraryCommand(string Name, string? Description, Guid UserId) : IRequest<Guid>;

public class CreateItinerary: IRequestHandler<CreateItineraryCommand, Guid>
{
    private readonly IItinerariesRepository _itinerariesRepository;
    
    public CreateItinerary(IItinerariesRepository itinerariesRepository)
    {
        _itinerariesRepository = itinerariesRepository;
    }

    public async Task<Guid> Handle(CreateItineraryCommand request, CancellationToken cancellationToken)
    {
        Itinerary itinerary = new Itinerary(request.Name, request.Description, request.UserId);

        await _itinerariesRepository.AddAsync(itinerary);

        return itinerary.Id;
    }
}