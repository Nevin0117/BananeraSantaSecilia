namespace SantaSecilia.Domain.Entities;
public class Worker
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string IdentificationNumber { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
