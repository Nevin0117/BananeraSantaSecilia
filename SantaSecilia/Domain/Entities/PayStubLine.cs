namespace SantaSecilia.Domain.Entities;

public class PayStubLine
{
    public int Id { get; set; }
    public int PayStubId { get; set; }
    public int DailyRecordId { get; set; }
    public decimal Amount { get; set; }
    public WeeklyPayStub PayStub { get; set; } = null!;
    public DailyRecord DailyRecord { get; set; } = null!;
}
