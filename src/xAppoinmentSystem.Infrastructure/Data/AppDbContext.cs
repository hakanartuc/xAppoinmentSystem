using Microsoft.EntityFrameworkCore;
using xAppoinmentSystem.Application.Interfaces;
using xAppoinmentSystem.Domain.Entities;

namespace xAppoinmentSystem.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ServiceProviderEntity> ServiceProviders { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }
    public DbSet<BusinessOwnershipEntity> BusinessOwnerships { get; set; }
    public DbSet<ServiceCategoryEntity> ServiceCategories { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<AppointmentEntity> Appointments { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<MediaEntity> MediaFiles { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }
    
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasDiscriminator<string>("UserType")
            .HasValue<CustomerEntity>("Customer")
            .HasValue<ServiceProviderEntity>("ServiceProvider");

        modelBuilder.Entity<UserEntity>().ToTable("Users");
        modelBuilder.Entity<CustomerEntity>();
        modelBuilder.Entity<ServiceProviderEntity>();

        modelBuilder.Entity<BusinessEntity>(entity =>
        {
            entity.HasOne(b => b.LocationEntity)
                  .WithMany()
                  .HasForeignKey(b => b.LocationId);

            entity.Property(b => b.AverageRating)
                  .HasPrecision(3, 2);
        });

        modelBuilder.Entity<ServiceEntity>(entity =>
        {
            entity.Property(s => s.Price)
                  .HasPrecision(10, 2);

            entity.HasOne(s => s.ServiceProviderEntity)
                  .WithMany(sp => sp.Services)
                  .HasForeignKey(s => s.ServiceProviderId);

            entity.HasOne(s => s.ServiceCategoryEntity)
                  .WithMany(sc => sc.Services)
                  .HasForeignKey(s => s.ServiceCategoryId);
        });

        modelBuilder.Entity<AppointmentEntity>(entity =>
        {
            entity.Property(a => a.Price)
                  .HasPrecision(10, 2);

            entity.HasOne(a => a.CustomerEntity)
                  .WithMany(c => c.Appointments)
                  .HasForeignKey(a => a.CustomerId);

            entity.HasOne(a => a.ServiceProviderEntity)
                  .WithMany(sp => sp.Appointments)
                  .HasForeignKey(a => a.ServiceProviderId);

            entity.HasOne(a => a.ServiceEntity)
                  .WithMany(s => s.Appointments)
                  .HasForeignKey(a => a.ServiceId);
        });

        modelBuilder.Entity<ReviewEntity>(entity =>
        {
            entity.HasOne(r => r.CustomerEntity)
                  .WithMany(c => c.Reviews)
                  .HasForeignKey(r => r.CustomerId);

            entity.HasOne(r => r.ServiceProvider)
                  .WithMany(sp => sp.Reviews)
                  .HasForeignKey(r => r.ServiceProviderId);

            entity.HasOne(r => r.Business)
                  .WithMany(b => b.Reviews)
                  .HasForeignKey(r => r.BusinessId);

            entity.HasOne(r => r.Appointment)
                  .WithMany()
                  .HasForeignKey(r => r.AppointmentId);
        });

        modelBuilder.Entity<BusinessOwnershipEntity>(entity =>
        {
            entity.HasOne(bo => bo.BusinessEntity)
                  .WithMany(b => b.BusinessOwnerships)
                  .HasForeignKey(bo => bo.BusinessId);

            entity.HasOne(bo => bo.Owner)
                  .WithMany()
                  .HasForeignKey(bo => bo.OwnerId);
        });

        modelBuilder.Entity<ServiceProviderEntity>(entity =>
        {
            entity.Property(sp => sp.AverageRating)
                  .HasPrecision(3, 2);

            entity.HasOne(sp => sp.Business)
                  .WithMany(b => b.ServiceProviders)
                  .HasForeignKey(sp => sp.BusinessId);
        });

        modelBuilder.Entity<MediaEntity>(entity =>
        {
            entity.HasOne(m => m.ServiceProvider)
                  .WithMany(sp => sp.MediaFiles)
                  .HasForeignKey(m => m.ServiceProviderId);

            entity.HasOne(m => m.Business)
                  .WithMany()
                  .HasForeignKey(m => m.BusinessId);
        });

        modelBuilder.Entity<LocationEntity>(entity =>
        {
            entity.Property(l => l.Latitude)
                  .HasPrecision(10, 8);

            entity.Property(l => l.Longitude)
                  .HasPrecision(11, 8);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}