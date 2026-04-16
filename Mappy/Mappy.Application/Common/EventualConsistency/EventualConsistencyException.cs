namespace Mappy.Application.Common.EventualConsistency;

public class EventualConsistencyException: Exception
{
    public EventualConsistencyException(string message): base(message) { }
}