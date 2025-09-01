using Microsoft.EntityFrameworkCore;
using xAppoinmentSystem.Domain.Entities;

namespace xAppoinmentSystem.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<CustomerEntity> Customers { get; set; }
    DbSet<ServiceProviderEntity> ServiceProviders { get; set; }
    DbSet<BusinessEntity> Businesses { get; set; }
    DbSet<BusinessOwnershipEntity> BusinessOwnerships { get; set; }
    DbSet<ServiceCategoryEntity> ServiceCategories { get; set; }
    DbSet<ServiceEntity> Services { get; set; }
    DbSet<AppointmentEntity> Appointments { get; set; }
    DbSet<ReviewEntity> Reviews { get; set; }
    DbSet<MediaEntity> MediaFiles { get; set; }
    DbSet<LocationEntity> Locations { get; set; }
    
    DbSet<UserEntity> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}