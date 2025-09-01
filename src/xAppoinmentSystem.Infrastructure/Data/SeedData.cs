using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xAppoinmentSystem.Domain.Entities;
using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Database already seeded");
                return;
            }

            logger.LogInformation("Seeding database...");

            // Seed Locations
            var locations = new List<LocationEntity>
            {
                new LocationEntity
                {
                    Latitude = 41.0082,
                    Longitude = 28.9784,
                    Address = "Taksim Meydanı No:1",
                    City = "İstanbul",
                    District = "Beyoğlu",
                    PostalCode = "34435"
                },
                new LocationEntity
                {
                    Latitude = 41.0055,
                    Longitude = 28.9769,
                    Address = "İstiklal Caddesi No:45",
                    City = "İstanbul",
                    District = "Beyoğlu",
                    PostalCode = "34433"
                },
                new LocationEntity
                {
                    Latitude = 39.9334,
                    Longitude = 32.8597,
                    Address = "Kızılay Meydanı No:12",
                    City = "Ankara",
                    District = "Çankaya",
                    PostalCode = "06420"
                }
            };
            await context.Locations.AddRangeAsync(locations);
            await context.SaveChangesAsync();

            // Seed Service Categories
            var categories = new List<ServiceCategoryEntity>
            {
                new ServiceCategoryEntity
                {
                    Name = "Saç Bakımı",
                    Description = "Saç kesimi, yıkama ve şekillendirme hizmetleri",
                    SortOrder = 1,
                    IsActive = true
                },
                new ServiceCategoryEntity
                {
                    Name = "Sakal Bakımı",
                    Description = "Sakal kesimi ve şekillendirme hizmetleri",
                    SortOrder = 2,
                    IsActive = true
                },
                new ServiceCategoryEntity
                {
                    Name = "Cilt Bakımı",
                    Description = "Yüz temizliği ve cilt bakım hizmetleri",
                    SortOrder = 3,
                    IsActive = true
                }
            };
            await context.ServiceCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Seed Businesses
            var businesses = new List<BusinessEntity>
            {
                new BusinessEntity
                {
                    Name = "Elite Kuaför",
                    Description = "Modern kuaförlük hizmetleri",
                    PhoneNumber = "+90532123456",
                    Email = "info@elitekuafor.com",
                    LocationId = locations[0].Id,
                    AverageRating = 4.5,
                    ReviewCount = 25,
                    IsActive = true
                },
                new BusinessEntity
                {
                    Name = "Gentleman's Choice",
                    Description = "Erkek kuaförlüğü uzmanı",
                    PhoneNumber = "+90532654321",
                    Email = "contact@gentlemans.com",
                    LocationId = locations[1].Id,
                    AverageRating = 4.8,
                    ReviewCount = 18,
                    IsActive = true
                },
                new BusinessEntity
                {
                    Name = "Style Center Ankara",
                    Description = "Ankara'nın en iyi kuaförü",
                    PhoneNumber = "+90312987654",
                    Email = "info@stylecenter.com",
                    LocationId = locations[2].Id,
                    AverageRating = 4.3,
                    ReviewCount = 31,
                    IsActive = true
                }
            };
            await context.Businesses.AddRangeAsync(businesses);
            await context.SaveChangesAsync();

            // Seed Service Providers
            var providers = new List<ServiceProviderEntity>
            {
                new ServiceProviderEntity
                {
                    PhoneNumber = "+90533111222",
                    FirstName = "Mehmet",
                    LastName = "Yılmaz",
                    Email = "mehmet.yilmaz@email.com",
                    Bio = "10 yıllık deneyimli kuaför",
                    AverageRating = 4.7,
                    ReviewCount = 45,
                    IsActive = true,
                    BusinessId = businesses[0].Id,
                    Status = ProviderStatus.Approved,
                    IsPhoneVerified = true
                },
                new ServiceProviderEntity
                {
                    PhoneNumber = "+90533222333",
                    FirstName = "Ali",
                    LastName = "Demir",
                    Email = "ali.demir@email.com",
                    Bio = "Sakal ve saç uzmanı",
                    AverageRating = 4.9,
                    ReviewCount = 32,
                    IsActive = true,
                    BusinessId = businesses[1].Id,
                    Status = ProviderStatus.Approved,
                    IsPhoneVerified = true
                },
                new ServiceProviderEntity
                {
                    PhoneNumber = "+90312333444",
                    FirstName = "Ayşe",
                    LastName = "Kaya",
                    Email = "ayse.kaya@email.com",
                    Bio = "Kadın ve erkek kuaförü",
                    AverageRating = 4.4,
                    ReviewCount = 28,
                    IsActive = true,
                    BusinessId = businesses[2].Id,
                    Status = ProviderStatus.Approved,
                    IsPhoneVerified = true
                }
            };
            await context.ServiceProviders.AddRangeAsync(providers);
            await context.SaveChangesAsync();

            // Seed Customers
            var customers = new List<CustomerEntity>
            {
                new CustomerEntity
                {
                    PhoneNumber = "+90544111222",
                    FirstName = "Emre",
                    LastName = "Özkan",
                    Email = "emre.ozkan@email.com",
                    IsPhoneVerified = true,
                    IsActive = true
                },
                new CustomerEntity
                {
                    PhoneNumber = "+90544222333",
                    FirstName = "Seda",
                    LastName = "Yıldız",
                    Email = "seda.yildiz@email.com",
                    IsPhoneVerified = true,
                    IsActive = true
                }
            };
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            // Seed Services
            var services = new List<ServiceEntity>
            {
                new ServiceEntity
                {
                    Name = "Erkek Saç Kesimi",
                    Description = "Profesyonel erkek saç kesimi",
                    Price = 100m,
                    DurationInMinutes = 30,
                    ServiceProviderId = providers[0].Id,
                    ServiceCategoryId = categories[0].Id,
                    IsActive = true
                },
                new ServiceEntity
                {
                    Name = "Sakal Düzeltme",
                    Description = "Sakal kesimi ve şekillendirme",
                    Price = 50m,
                    DurationInMinutes = 20,
                    ServiceProviderId = providers[1].Id,
                    ServiceCategoryId = categories[1].Id,
                    IsActive = true
                },
                new ServiceEntity
                {
                    Name = "Kadın Saç Kesimi",
                    Description = "Trend kadın saç kesimi",
                    Price = 200m,
                    DurationInMinutes = 60,
                    ServiceProviderId = providers[2].Id,
                    ServiceCategoryId = categories[0].Id,
                    IsActive = true
                },
                new ServiceEntity
                {
                    Name = "Yüz Temizliği",
                    Description = "Derin yüz temizliği",
                    Price = 150m,
                    DurationInMinutes = 45,
                    ServiceProviderId = providers[0].Id,
                    ServiceCategoryId = categories[2].Id,
                    IsActive = true
                }
            };
            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}