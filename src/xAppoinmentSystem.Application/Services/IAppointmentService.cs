using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Models;

namespace xAppoinmentSystem.Application.Services;

public interface IAppointmentService
{
    Task<ServiceResult<Appointment?>> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Appointment> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<Appointment?> ConfirmAppointmentAsync(int appointmentId, int serviceProviderId, CancellationToken cancellationToken = default);
    Task<Appointment?> CancelAppointmentAsync(int appointmentId, int userId, string reason, CancellationToken cancellationToken = default);
    Task<Appointment?> CompleteAppointmentAsync(int appointmentId, int serviceProviderId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetAppointmentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetAppointmentsByServiceProviderAsync(int serviceProviderId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetAppointmentsByDateRangeAsync(int serviceProviderId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<bool> IsTimeSlotAvailableAsync(int serviceProviderId, DateTime appointmentDateTime, int durationInMinutes, CancellationToken cancellationToken = default);
}