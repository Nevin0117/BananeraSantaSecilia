namespace SantaSecilia.Domain.Entities;

/// <summary>
/// Actividades que pueden realizar los trabajadores
/// </summary>
public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
