namespace xAppoinmentSystem.Application.DTOs.Requests;

public class CreateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public int ServiceProviderId { get; set; }
    public int ServiceCategoryId { get; set; }
}