namespace SantaSecilia.Domain.Entities;

/// <summary>
/// Línea de registro diario - una actividad específica con horas trabajadas en un lote
/// </summary>
public class DailyRecordLine
{
    public int Id { get; set; }
    public int DailyRecordId { get; set; }
    public int ActivityId { get; set; }
    public int LotId { get; set; }
    public int Hours { get; set; }
    public decimal RateSnapshot { get; set; }
    public decimal SubtotalSnapshot { get; set; }
    public DailyRecord DailyRecord { get; set; } = null!;
    public Activity Activity { get; set; } = null!;
    public Lot Lot { get; set; } = null!;
}
