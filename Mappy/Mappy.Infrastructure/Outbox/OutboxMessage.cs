namespace Mappy.Infrastructure.Outbox;

public class OutboxMessage
{
    private uint _xmin;
    
    public int Id { get; }
    public DateTime OccurredOn { get; }
    public bool IsProcessed { get; set; }
    public string Type { get; }
    public string Body { get; }

    public OutboxMessage(string type, string body)
    {
        OccurredOn = DateTime.UtcNow;
        IsProcessed = false;
        Type = type;
        Body = body;
    }
    
    private OutboxMessage() { }
}