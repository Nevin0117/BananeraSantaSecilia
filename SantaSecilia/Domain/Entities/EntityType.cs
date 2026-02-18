namespace SantaSecilia.Domain.Entities;

public class EntityType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<AuditEvent> AuditEvents { get; set; } = [];
}
