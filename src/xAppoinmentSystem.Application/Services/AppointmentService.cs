using Mapster;
using Microsoft.EntityFrameworkCore;
using xAppoinmentSystem.Application.DTOs;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Interfaces;
using xAppoinmentSystem.Application.Models;
using xAppoinmentSystem.Domain.Entities;
using xAppoinmentSystem.Domain.Enums;

namespace xAppoinmentSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppDbContext _context;
    private readonly ITieredCacheService _cache;

    public AppointmentService(IAppDbContext context, ITieredCacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ServiceResult<Appointment?>> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"appointment:{id}";
        var cached = await _cache.GetAsync<Appointment>(cacheKey, cancellationToken);
        if (cached != null) return ServiceResult<Appointment?>.From(cached);

        var appointment = await _context.Appointments
            .Include(a => a.CustomerEntity)
            .Include(a => a.ServiceProviderEntity)
            .Include(a => a.ServiceEntity)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

        if (appointment == null) return ServiceResult<Appointment?>.From(null);

        var dto = appointment.Adapt<Appointment>();
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10), cancellationToken);
        
        return ServiceResult<Appointment?>.From(dto);
    }

    public async Task<Appointment> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == request.ServiceId && !s.IsDeleted, cancellationToken);

        if (service == null)
            throw new ArgumentException("Service not found");

        var appointment = new AppointmentEntity
        {
            CustomerId = request.CustomerId,
            ServiceProviderId = request.ServiceProviderId,
            ServiceId = request.ServiceId,
            AppointmentDateTime = request.AppointmentDateTime,
            DurationInMinutes = service.DurationInMinutes,
            Price = service.Price,
            CustomerNotes = request.CustomerNotes,
            Status = AppointmentStatus.Pending
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("appointment:", cancellationToken);
        await _cache.RemoveByPrefixAsync("appointments:", cancellationToken);
        
        return appointment.Adapt<Appointment>();
    }

    public async Task<Appointment?> ConfirmAppointmentAsync(int appointmentId, int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && 
                                    a.ServiceProviderId == serviceProviderId && 
                                    !a.IsDeleted, cancellationToken);

        if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            return null;

        appointment.Status = AppointmentStatus.Confirmed;
        appointment.ConfirmedAt = DateTime.UtcNow;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"appointment:{appointmentId}", cancellationToken);
        
        return appointment.Adapt<Appointment>();
    }

    public async Task<Appointment?> CancelAppointmentAsync(int appointmentId, int userId, string reason, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && 
                                    (a.CustomerId == userId || a.ServiceProviderId == userId) && 
                                    !a.IsDeleted, cancellationToken);

        if (appointment == null || appointment.Status == AppointmentStatus.Completed)
            return null;

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancelledAt = DateTime.UtcNow;
        appointment.CancellationReason = reason;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"appointment:{appointmentId}", cancellationToken);
        
        return appointment.Adapt<Appointment>();
    }

    public async Task<Appointment?> CompleteAppointmentAsync(int appointmentId, int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && 
                                    a.ServiceProviderId == serviceProviderId && 
                                    !a.IsDeleted, cancellationToken);

        if (appointment == null || appointment.Status != AppointmentStatus.Confirmed)
            return null;

        appointment.Status = AppointmentStatus.Completed;
        appointment.CompletedAt = DateTime.UtcNow;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"appointment:{appointmentId}", cancellationToken);
        
        return appointment.Adapt<Appointment>();
    }

    public async Task<List<Appointment>> GetAppointmentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"appointments:customer:{customerId}";
        var cached = await _cache.GetAsync<List<Appointment>>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var appointments = await _context.Appointments
            .Include(a => a.ServiceProviderEntity)
            .Include(a => a.ServiceEntity)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .Where(a => a.CustomerId == customerId && !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var dtos = appointments.Adapt<List<Appointment>>();
        await _cache.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(5), cancellationToken);
        
        return dtos;
    }

    public async Task<List<Appointment>> GetAppointmentsByServiceProviderAsync(int serviceProviderId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"appointments:provider:{serviceProviderId}";
        var cached = await _cache.GetAsync<List<Appointment>>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var appointments = await _context.Appointments
            .Include(a => a.CustomerEntity)
            .Include(a => a.ServiceEntity)
            .ThenInclude(s => s.ServiceCategoryEntity)
            .Where(a => a.ServiceProviderId == serviceProviderId && !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        var dtos = appointments.Adapt<List<Appointment>>();
        await _cache.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(5), cancellationToken);
        
        return dtos;
    }

    public async Task<List<Appointment>> GetAppointmentsByDateRangeAsync(int serviceProviderId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var appointments = await _context.Appointments
            .Include(a => a.CustomerEntity)
            .Include(a => a.ServiceEntity)
            .Where(a => a.ServiceProviderId == serviceProviderId && 
                       a.AppointmentDateTime >= startDate && 
                       a.AppointmentDateTime <= endDate && 
                       !a.IsDeleted)
            .OrderBy(a => a.AppointmentDateTime)
            .ToListAsync(cancellationToken);

        return appointments.Adapt<List<Appointment>>();
    }

    public async Task<bool> IsTimeSlotAvailableAsync(int serviceProviderId, DateTime appointmentDateTime, int durationInMinutes, CancellationToken cancellationToken = default)
    {
        var endTime = appointmentDateTime.AddMinutes(durationInMinutes);

        var conflictingAppointment = await _context.Appointments
            .AnyAsync(a => a.ServiceProviderId == serviceProviderId && 
                          !a.IsDeleted && 
                          a.Status != AppointmentStatus.Cancelled &&
                          ((a.AppointmentDateTime < endTime && 
                            a.AppointmentDateTime.AddMinutes(a.DurationInMinutes) > appointmentDateTime)), 
                     cancellationToken);

        return !conflictingAppointment;
    }
}