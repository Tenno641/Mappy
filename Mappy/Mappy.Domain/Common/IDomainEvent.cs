using System.Text.Json.Serialization;
using Mappy.Domain.Itineraries.Events;
using MediatR;

namespace Mappy.Domain.Common;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(StopAddedEvent), "stopAdded")]
public interface IDomainEvent : INotification;