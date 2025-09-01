namespace xAppoinmentSystem.Application.DTOs;

public class Review
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? ServiceProviderId { get; set; }
    public int? BusinessId { get; set; }
    public int? AppointmentId { get; set; }
    public string Target { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVisible { get; set; }
    public DateTime CreatedAt { get; set; }
}