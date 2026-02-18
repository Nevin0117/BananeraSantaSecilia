namespace SantaSecilia.Domain.Entities;
public class WeeklyClosing
{
    public int Id { get; set; }
    public DateOnly WeekStart { get; set; }
    public DateOnly WeekEnd { get; set; }
    public string Status { get; set; } = "";    // "CLOSED" | "VOIDED"  (CHECK enforced in DbContext)
    public decimal SsRateSnapshot { get; set; }
    public decimal SeRateSnapshot { get; set; }
    public decimal UnionDuesSnapshot { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Notes { get; set; }
    public User CreatedByUser { get; set; } = null!;
}
