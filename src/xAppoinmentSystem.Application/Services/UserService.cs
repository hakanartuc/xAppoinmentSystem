using Mapster;
using Microsoft.EntityFrameworkCore;
using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Interfaces;
using xAppoinmentSystem.Domain.Entities;

namespace xAppoinmentSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IAppDbContext _context;
    private readonly ITieredCacheService _cache;

    public UserService(IAppDbContext context, ITieredCacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ServiceProvider?> GetServiceProviderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"serviceprovider:{id}";
        var cached = await _cache.GetAsync<ServiceProvider>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var provider = await _context.ServiceProviders
            .Include(sp => sp.Business)
            .ThenInclude(b => b != null ? b.LocationEntity : null)
            .Include(sp => sp.Services)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .Include(sp => sp.MediaFiles)
            .FirstOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted, cancellationToken);

        if (provider == null) return null;

        var dto = provider.Adapt<ServiceProvider>();
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(15), cancellationToken);
        
        return dto;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"customer:{id}";
        var cached = await _cache.GetAsync<Customer>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (customer == null) return null;

        var dto = customer.Adapt<Customer>();
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(15), cancellationToken);
        
        return dto;
    }

    public async Task<ServiceProvider> CreateServiceProviderAsync(CreateServiceProviderRequest request, CancellationToken cancellationToken = default)
    {
        var provider = request.Adapt<ServiceProviderEntity>();
        
        _context.ServiceProviders.Add(provider);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("serviceprovider:", cancellationToken);
        
        return provider.Adapt<ServiceProvider>();
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = request.Adapt<CustomerEntity>();
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("customer:", cancellationToken);
        
        return customer.Adapt<Customer>();
    }

    public async Task<ServiceProvider?> GetServiceProviderByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var provider = await _context.ServiceProviders
            .Include(sp => sp.Business)
            .ThenInclude(b => b != null ? b.LocationEntity : null)
            .Include(sp => sp.Services)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .FirstOrDefaultAsync(sp => sp.PhoneNumber == phoneNumber && !sp.IsDeleted, cancellationToken);

        return provider?.Adapt<ServiceProvider>();
    }

    public async Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && !c.IsDeleted, cancellationToken);

        return customer?.Adapt<Customer>();
    }

    public async Task<List<ServiceProvider>> GetServiceProvidersByLocationAsync(double latitude, double longitude, double radiusKm, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"providers:location:{latitude}:{longitude}:{radiusKm}";
        var cached = await _cache.GetAsync<List<ServiceProvider>>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var providers = await _context.ServiceProviders
            .Include(sp => sp.Business)
            .ThenInclude(b => b != null ? b.LocationEntity : null)
            .Include(sp => sp.Services)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .Where(sp => !sp.IsDeleted && sp.IsActive && 
                         sp.Business != null && sp.Business.LocationEntity != null &&
                         (6371 * Math.Acos(Math.Cos(Math.PI * latitude / 180) * 
                                         Math.Cos(Math.PI * sp.Business.LocationEntity.Latitude / 180) * 
                                         Math.Cos(Math.PI * sp.Business.LocationEntity.Longitude / 180 - Math.PI * longitude / 180) + 
                                         Math.Sin(Math.PI * latitude / 180) * 
                                         Math.Sin(Math.PI * sp.Business.LocationEntity.Latitude / 180))) <= radiusKm)
            .OrderBy(sp => sp.AverageRating)
            .ToListAsync(cancellationToken);

        var dtos = providers.Adapt<List<ServiceProvider>>();
        await _cache.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(10), cancellationToken);
        
        return dtos;
    }
}