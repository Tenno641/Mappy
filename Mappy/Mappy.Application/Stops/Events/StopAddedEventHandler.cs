using Mappy.Application.Common.Interfaces;
using Mappy.Domain.Itineraries.Events;
using MediatR;

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
        await _stopsRepository.CreateStopAsync(notification.Stop);
    }
}