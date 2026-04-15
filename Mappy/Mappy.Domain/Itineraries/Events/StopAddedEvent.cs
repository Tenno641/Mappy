using Mappy.Domain.Common;
using Mappy.Domain.Stops;

namespace Mappy.Domain.Itineraries.Events;

public record StopAddedEvent(Stop Stop): IDomainEvent;