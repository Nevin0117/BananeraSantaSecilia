namespace SantaSecilia.Domain.Entities;

public class RecordVoiding
{
    public int RecordId { get; set; }
    public int VoidedByUserId { get; set; }
    public string? VoidReason { get; set; }
    public DateTime VoidedAt { get; set; }
    public DailyRecord Record { get; set; } = null!;
    public User VoidedByUser { get; set; } = null!;
}
