namespace xAppoinmentSystem.Domain.Entities;

public class CustomerEntity : UserEntity
{
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
    public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
}