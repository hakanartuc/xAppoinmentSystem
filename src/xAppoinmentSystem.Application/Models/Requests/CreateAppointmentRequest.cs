namespace xAppoinmentSystem.Application.DTOs.Requests;

public class CreateAppointmentRequest
{
    public int CustomerId { get; set; }
    public int ServiceProviderId { get; set; }
    public int ServiceId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string? CustomerNotes { get; set; }
}