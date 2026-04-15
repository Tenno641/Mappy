using Mappy.Domain.Common;
using Mappy.Domain.Stops;

namespace Mappy.Domain.Itineraries;

public record StopAddedEvent(Stop Stop): IDomainEvent;