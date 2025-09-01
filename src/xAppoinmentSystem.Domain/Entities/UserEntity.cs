namespace xAppoinmentSystem.Domain.Entities;

public abstract class UserEntity : BaseEntity
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime? PhoneVerifiedAt { get; set; }
    public bool IsPhoneVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
}