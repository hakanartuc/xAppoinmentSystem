namespace xAppoinmentSystem.Application.DTOs.Requests;

public class CreateServiceProviderRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int? BusinessId { get; set; }
}