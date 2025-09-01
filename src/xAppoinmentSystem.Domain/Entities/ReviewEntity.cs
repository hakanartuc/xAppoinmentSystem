using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class ReviewEntity : BaseEntity
{
    public int CustomerId { get; set; }
    public CustomerEntity CustomerEntity { get; set; } = null!;
    public int? ServiceProviderId { get; set; }
    public ServiceProviderEntity? ServiceProvider { get; set; }
    public int? BusinessId { get; set; }
    public BusinessEntity? Business { get; set; }
    public int? AppointmentId { get; set; }
    public AppointmentEntity? Appointment { get; set; }
    public ReviewTarget Target { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVisible { get; set; } = true;
}