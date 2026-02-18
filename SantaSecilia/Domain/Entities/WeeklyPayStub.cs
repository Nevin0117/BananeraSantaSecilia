namespace SantaSecilia.Domain.Entities;
public class WeeklyPayStub
{
    public int Id { get; set; }
    public int WeeklyClosingId { get; set; }
    public int WorkerId { get; set; }
    public int TotalHours { get; set; }
    public decimal GrossEarnings { get; set; }
    public decimal SsDeduction { get; set; }
    public decimal SeDeduction { get; set; }
    public decimal UnionDues { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string? PdfPath { get; set; }
    public WeeklyClosing WeeklyClosing { get; set; } = null!;
    public Worker Worker { get; set; } = null!;
    public ICollection<PayStubLine> Lines { get; set; } = [];
}
