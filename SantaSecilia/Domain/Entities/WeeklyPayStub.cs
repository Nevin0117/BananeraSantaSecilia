namespace SantaSecilia.Domain.Entities;
public class WeeklyPayStub
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public DateOnly WeekStart { get; set; }
    public DateOnly WeekEnd { get; set; }
    public int TotalHours { get; set; }
    public decimal GrossEarnings { get; set; }
    public decimal SocialSecurityDeduction { get; set; }
    public decimal EducationInsuranceDeduction { get; set; }
    public decimal UnionDues { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? PdfPath { get; set; }
    public Worker Worker { get; set; } = null!;
}
