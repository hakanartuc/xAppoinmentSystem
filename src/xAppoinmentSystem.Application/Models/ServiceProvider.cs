namespace xAppoinmentSystem.Application.DTOs;

public class ServiceProvider : User
{
    public string Bio { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsActive { get; set; }
    public int? BusinessId { get; set; }
    public Business? Business { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<Service> Services { get; set; } = new();
    public List<Media> MediaFiles { get; set; } = new();
}