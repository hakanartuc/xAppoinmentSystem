namespace xAppoinmentSystem.Application.DTOs;

public class Appointment
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ServiceProviderId { get; set; }
    public ServiceProvider ServiceProvider { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public DateTime AppointmentDateTime { get; set; }
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CustomerNotes { get; set; }
    public string? ProviderNotes { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}