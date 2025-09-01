namespace xAppoinmentSystem.Domain.Entities;

public class ServiceCategoryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
    
    public ICollection<ServiceEntity> Services { get; set; } = new List<ServiceEntity>();
}