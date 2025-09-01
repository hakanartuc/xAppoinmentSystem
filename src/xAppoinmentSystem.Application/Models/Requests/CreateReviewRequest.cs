using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Application.DTOs.Requests;

public class CreateReviewRequest
{
    public int CustomerId { get; set; }
    public int? ServiceProviderId { get; set; }
    public int? BusinessId { get; set; }
    public int? AppointmentId { get; set; }
    public ReviewTarget Target { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}