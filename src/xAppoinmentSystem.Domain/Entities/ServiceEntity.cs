namespace xAppoinmentSystem.Domain.Entities;

public class ServiceEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public bool IsActive { get; set; } = true;
    public int ServiceProviderId { get; set; }
    public ServiceProviderEntity ServiceProviderEntity { get; set; } = null!;
    public int ServiceCategoryId { get; set; }
    public ServiceCategoryEntity ServiceCategoryEntity { get; set; } = null!;
    
    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
}