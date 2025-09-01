using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class ServiceProviderEntity : UserEntity
{
    public string Bio { get; set; } = string.Empty;
    public double AverageRating { get; set; } = 0.0;
    public int ReviewCount { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public int? BusinessId { get; set; }
    public BusinessEntity? Business { get; set; }
    public ProviderStatus Status { get; set; } = ProviderStatus.PendingApproval;
    
    public ICollection<ServiceEntity> Services { get; set; } = new List<ServiceEntity>();
    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
    public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
    public ICollection<MediaEntity> MediaFiles { get; set; } = new List<MediaEntity>();
}