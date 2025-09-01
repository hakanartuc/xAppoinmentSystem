namespace xAppoinmentSystem.Application.DTOs;

public class Customer : User
{
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; }
}