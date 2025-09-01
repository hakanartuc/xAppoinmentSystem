using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class AppointmentEntity : BaseEntity
{
    public int CustomerId { get; set; }
    public CustomerEntity CustomerEntity { get; set; } = null!;
    public int ServiceProviderId { get; set; }
    public ServiceProviderEntity ServiceProviderEntity { get; set; } = null!;
    public int ServiceId { get; set; }
    public ServiceEntity ServiceEntity { get; set; } = null!;
    public DateTime AppointmentDateTime { get; set; }
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? CustomerNotes { get; set; }
    public string? ProviderNotes { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
}