using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class Review : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? ServiceProviderId { get; set; }
    public ServiceProvider? ServiceProvider { get; set; }
    public int? BusinessId { get; set; }
    public Business? Business { get; set; }
    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    public ReviewTarget Target { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVisible { get; set; } = true;
}