namespace xAppoinmentSystem.Application.DTOs;

public class Business
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public Location Location { get; set; } = null!;
    public string? LogoImageUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ServiceProvider> ServiceProviders { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}