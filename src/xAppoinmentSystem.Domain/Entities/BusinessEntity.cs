namespace xAppoinmentSystem.Domain.Entities;

public class BusinessEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int LocationId { get; set; }
    public LocationEntity LocationEntity { get; set; } = null!;
    public string? LogoImageUrl { get; set; }
    public double AverageRating { get; set; } = 0.0;
    public int ReviewCount { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    
    public ICollection<BusinessOwnershipEntity> BusinessOwnerships { get; set; } = new List<BusinessOwnershipEntity>();
    public ICollection<ServiceProviderEntity> ServiceProviders { get; set; } = new List<ServiceProviderEntity>();
    public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
}