using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries.Events;
using MediatR;
using ErrorOr;

namespace Mappy.Application.Stops.Events;

public class StopAddedEventHandler: INotificationHandler<StopAddedEvent>
{
    private readonly IStopsRepository _stopsRepository;
    
    public StopAddedEventHandler(IStopsRepository stopsRepository)
    {
        _stopsRepository = stopsRepository;
    }

    public async Task Handle(StopAddedEvent notification, CancellationToken cancellationToken)
    {
        ErrorOr<Success> result = notification.Stop.SetItineraryId(notification.ItineraryId);

        if (result.IsError)
            return;
        // TODO: Eventual Consistency Exception
        
        await _stopsRepository.CreateStopAsync(notification.Stop);
    }
}