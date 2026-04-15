using ErrorOr;
using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries;
using Mappy.Domain.Stops;
using MediatR;

namespace Mappy.Application.Stops;

public record CreateStopCommand(string Name, string? ImageUri, Guid ItineraryId) : IRequest<ErrorOr<Guid>>;

public class CreateStop: IRequestHandler<CreateStopCommand, ErrorOr<Guid>>
{
    private readonly IItinerariesRepository _itinerariesRepository;
    
    public CreateStop(IItinerariesRepository itinerariesRepository)
    {
        _itinerariesRepository = itinerariesRepository;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateStopCommand request, CancellationToken cancellationToken)
    {
        Itinerary? itinerary = await _itinerariesRepository.GetByIdAsync(request.ItineraryId);
        
        if (itinerary is null)
            return Error.NotFound(description: "Itinerary is not found");
        
        Uri imageUri = new Uri(request.ImageUri); // TODO: null reference

        Stop stop = new Stop(request.Name, imageUri);
        
        ErrorOr<Success> result = itinerary.AddStop(stop);

        if (result.IsError)
            return result.Errors;
        
        await _itinerariesRepository.UpdateAsync(itinerary);

        return stop.Id;
    }
}