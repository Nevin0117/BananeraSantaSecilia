namespace SantaSecilia.Domain.Entities;

/// <summary>
/// Registro de trabajo diario - encabezado que agrupa las actividades de un trabajador en un día
/// </summary>
public class DailyRecord
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public DateTime WorkDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Worker Worker { get; set; } = null!;
    public ICollection<DailyRecordLine> Lines { get; set; } = [];
}
