namespace SantaSecilia.Domain.Entities;
public class Worker
{
    public int Id { get; set; }
    public int Code { get; set; }           // business employee number — maps to TrabajadorItem.Codigo
    public string FullName { get; set; } = "";
    public string IdNumber { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
