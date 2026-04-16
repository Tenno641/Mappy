using System.Text.Json.Serialization;

namespace Mappy.Domain.Common;

public class AuditableEntity: Entity
{
    [JsonIgnore]
    public string? CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime CreatedOn { get; set; }
    [JsonIgnore]
    public string? ModifiedBy { get; set; }
    [JsonIgnore]
    public DateTime ModifiedOn { get; set; }

    protected AuditableEntity(Guid? id = null): base(id)
    {
    }
}