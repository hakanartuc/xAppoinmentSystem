using Mapster;
using Microsoft.EntityFrameworkCore;
using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Interfaces;
using xAppoinmentSystem.Domain.Entities;
using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Application.Services;

public class BusinessService : IBusinessService
{
    private readonly IAppDbContext _context;
    private readonly ITieredCacheService _cache;

    public BusinessService(IAppDbContext context, ITieredCacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"business:{id}";
        var cached = await _cache.GetAsync<Business>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var business = await _context.Businesses
            .Include(b => b.LocationEntity)
            .Include(b => b.ServiceProviders)
            .Include(b => b.Reviews)
            .ThenInclude(r => r.CustomerEntity)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);

        if (business == null) return null;

        var dto = business.Adapt<Business>();
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(15), cancellationToken);
        
        return dto;
    }

    public async Task<Business> CreateBusinessAsync(CreateBusinessRequest request, CancellationToken cancellationToken = default)
    {
        var location = new LocationEntity
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Address = request.Address,
            City = request.City,
            District = request.District,
            PostalCode = request.PostalCode
        };

        _context.Locations.Add(location);
        await _context.SaveChangesAsync(cancellationToken);

        var business = new BusinessEntity
        {
            Name = request.Name,
            Description = request.Description,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Website = request.Website,
            LocationId = location.Id
        };

        _context.Businesses.Add(business);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("business:", cancellationToken);
        
        return business.Adapt<Business>();
    }

    public async Task<List<Business>> GetBusinessesByLocationAsync(double latitude, double longitude, double radiusKm, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"businesses:location:{latitude}:{longitude}:{radiusKm}";
        var cached = await _cache.GetAsync<List<Business>>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var businesses = await _context.Businesses
            .Include(b => b.LocationEntity)
            .Include(b => b.ServiceProviders)
            .Where(b => !b.IsDeleted && b.IsActive &&
                       (6371 * Math.Acos(Math.Cos(Math.PI * latitude / 180) * 
                                       Math.Cos(Math.PI * b.LocationEntity.Latitude / 180) * 
                                       Math.Cos(Math.PI * b.LocationEntity.Longitude / 180 - Math.PI * longitude / 180) + 
                                       Math.Sin(Math.PI * latitude / 180) * 
                                       Math.Sin(Math.PI * b.LocationEntity.Latitude / 180))) <= radiusKm)
            .OrderByDescending(b => b.AverageRating)
            .ToListAsync(cancellationToken);

        var dtos = businesses.Adapt<List<Business>>();
        await _cache.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(10), cancellationToken);
        
        return dtos;
    }

    public async Task<Business?> UpdateBusinessAsync(int id, CreateBusinessRequest request, CancellationToken cancellationToken = default)
    {
        var business = await _context.Businesses
            .Include(b => b.LocationEntity)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);

        if (business == null) return null;

        business.Name = request.Name;
        business.Description = request.Description;
        business.PhoneNumber = request.PhoneNumber;
        business.Email = request.Email;
        business.Website = request.Website;
        business.UpdatedAt = DateTime.UtcNow;

        if (business.LocationEntity != null)
        {
            business.LocationEntity.Latitude = request.Latitude;
            business.LocationEntity.Longitude = request.Longitude;
            business.LocationEntity.Address = request.Address;
            business.LocationEntity.City = request.City;
            business.LocationEntity.District = request.District;
            business.LocationEntity.PostalCode = request.PostalCode;
            business.LocationEntity.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"business:{id}", cancellationToken);

        return business.Adapt<Business>();
    }

    public async Task<bool> RequestBusinessOwnershipAsync(int businessId, int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var existingOwnership = await _context.BusinessOwnerships
            .AnyAsync(bo => bo.BusinessId == businessId && 
                           bo.OwnerId == serviceProviderId && 
                           !bo.IsDeleted, cancellationToken);

        if (existingOwnership) return false;

        var ownership = new BusinessOwnershipEntity
        {
            BusinessId = businessId,
            OwnerId = serviceProviderId,
            Status = OwnershipStatus.PendingApproval
        };

        _context.BusinessOwnerships.Add(ownership);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ApproveBusinessOwnershipAsync(int ownershipId, int ownerId, CancellationToken cancellationToken = default)
    {
        var ownership = await _context.BusinessOwnerships
            .FirstOrDefaultAsync(bo => bo.Id == ownershipId && 
                                      bo.OwnerId == ownerId && 
                                      bo.Status == OwnershipStatus.PendingApproval && 
                                      !bo.IsDeleted, cancellationToken);

        if (ownership == null) return false;

        ownership.Status = OwnershipStatus.Approved;
        ownership.ApprovedAt = DateTime.UtcNow;
        ownership.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("business:", cancellationToken);

        return true;
    }
}