namespace SantaSecilia.Domain.Entities;

/// <summary>
/// Jornaleros de la finca
/// </summary>
public class Worker
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string IdentificationNumber { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // --- NUEVA PROPIEDAD PARA UI ---
    // Esta propiedad no se guarda en la base de datos (es calculada)
    public string DisplayInfo => $"{FullName} — {IdentificationNumber}";
}
