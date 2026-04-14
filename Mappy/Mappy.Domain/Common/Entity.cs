namespace Mappy.Domain.Common;

public class Entity
{
    public Guid Id { get; }

    protected Entity(Guid? id = null)
    {
        Id = id ?? Guid.CreateVersion7();
    }
}