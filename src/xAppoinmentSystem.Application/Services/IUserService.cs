using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;

namespace xAppoinmentSystem.Application.Services;

public interface IUserService
{
    Task<ServiceProvider?> GetServiceProviderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceProvider> CreateServiceProviderAsync(CreateServiceProviderRequest request, CancellationToken cancellationToken = default);
    Task<Customer> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<ServiceProvider?> GetServiceProviderByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<List<ServiceProvider>> GetServiceProvidersByLocationAsync(double latitude, double longitude, double radiusKm, CancellationToken cancellationToken = default);
}