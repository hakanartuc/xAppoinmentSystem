using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;

namespace xAppoinmentSystem.Application.Services;

public interface IBusinessService
{
    Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Business> CreateBusinessAsync(CreateBusinessRequest request, CancellationToken cancellationToken = default);
    Task<List<Business>> GetBusinessesByLocationAsync(double latitude, double longitude, double radiusKm, CancellationToken cancellationToken = default);
    Task<Business?> UpdateBusinessAsync(int id, CreateBusinessRequest request, CancellationToken cancellationToken = default);
    Task<bool> RequestBusinessOwnershipAsync(int businessId, int serviceProviderId, CancellationToken cancellationToken = default);
    Task<bool> ApproveBusinessOwnershipAsync(int ownershipId, int ownerId, CancellationToken cancellationToken = default);
}