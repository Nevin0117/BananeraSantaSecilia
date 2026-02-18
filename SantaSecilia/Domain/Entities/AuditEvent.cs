namespace SantaSecilia.Domain.Entities;

public class AuditEvent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EntityTypeId { get; set; }
    public int EntityId { get; set; }
    public string Action { get; set; } = "";    // "CREATE" | "EDIT" | "VOID" | "CLOSE_WEEK" | "GENERATE_PDF"
    public string? Details { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; } = null!;
    public EntityType EntityType { get; set; } = null!;
}
