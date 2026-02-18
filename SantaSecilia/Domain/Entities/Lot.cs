namespace SantaSecilia.Domain.Entities;
public class Lot
{
    public int Id { get; set; }
    public int Code { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
