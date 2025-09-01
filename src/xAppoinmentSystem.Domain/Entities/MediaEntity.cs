using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Domain.Entities;

public class MediaEntity : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeInBytes { get; set; }
    public MediaType MediaType { get; set; }
    public int? ServiceProviderId { get; set; }
    public ServiceProviderEntity? ServiceProvider { get; set; }
    public int? BusinessId { get; set; }
    public BusinessEntity? Business { get; set; }
    public bool IsActive { get; set; } = true;
}