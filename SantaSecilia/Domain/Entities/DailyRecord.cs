namespace SantaSecilia.Domain.Entities;

public class DailyRecord
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public DateTime WorkDate { get; set; }
    public int ActivityId { get; set; }
    public int LotId { get; set; }
    public int Hours { get; set; }
    public decimal RateSnapshot { get; set; }
    public decimal SubtotalSnapshot { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? UpdatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsLocked { get; set; } = false;
    public int? WeeklyClosingId { get; set; }
    public bool AllowDuplicate { get; set; } = false;
    public Worker Worker { get; set; } = null!;
    public Activity Activity { get; set; } = null!;
    public Lot Lot { get; set; } = null!;
    public RecordVoiding? Voiding { get; set; }
}
