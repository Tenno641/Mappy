namespace Mappy.Domain.Common;

public class Entity
{
    public Guid Id { get; }

    public List<IDomainEvent> DomainEvents = [];

    protected Entity(Guid? id = null)
    {
        Id = id ?? Guid.CreateVersion7();
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        List<IDomainEvent> temp = DomainEvents.ToList(); 
        DomainEvents.Clear();
        return temp;
    }
}