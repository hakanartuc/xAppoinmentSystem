namespace xAppoinmentSystem.Application.DTOs;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public bool IsActive { get; set; }
    public int ServiceProviderId { get; set; }
    public ServiceCategory ServiceCategory { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}