namespace Mappy.Domain.Common;

public class AuditableEntity: Entity
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }

    protected AuditableEntity(Guid? id = null): base(id)
    {
    }
}