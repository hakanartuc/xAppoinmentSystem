using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class BusinessOwnershipEntity : BaseEntity
{
    public int BusinessId { get; set; }
    public BusinessEntity BusinessEntity { get; set; } = null!;
    public int OwnerId { get; set; }
    public ServiceProviderEntity Owner { get; set; } = null!;
    public OwnershipStatus Status { get; set; } = OwnershipStatus.PendingApproval;
    public DateTime? ApprovedAt { get; set; }
    public string? Notes { get; set; }
}