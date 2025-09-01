using Microsoft.AspNetCore.Mvc;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Services;

namespace xAppoinmentSystem.API.Controllers;

public class AppointmentController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id, CancellationToken cancellationToken)
    {
        var serviceResult = await _appointmentService.GetAppointmentByIdAsync(id, cancellationToken);
        
        return BaseResponse(serviceResult);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CreateAppointmentAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> ConfirmAppointment(int id, [FromBody] int serviceProviderId, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.ConfirmAppointmentAsync(id, serviceProviderId, cancellationToken);
        if (appointment == null) return NotFound();
        
        return Ok(appointment);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelAppointment(int id, [FromBody] CancelAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CancelAppointmentAsync(id, request.UserId, request.Reason, cancellationToken);
        if (appointment == null) return NotFound();
        
        return Ok(appointment);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteAppointment(int id, [FromBody] int serviceProviderId, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CompleteAppointmentAsync(id, serviceProviderId, cancellationToken);
        if (appointment == null) return NotFound();
        
        return Ok(appointment);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerAppointments(int customerId, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAppointmentsByCustomerAsync(customerId, cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("service-provider/{serviceProviderId}")]
    public async Task<IActionResult> GetServiceProviderAppointments(int serviceProviderId, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAppointmentsByServiceProviderAsync(serviceProviderId, cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("service-provider/{serviceProviderId}/schedule")]
    public async Task<IActionResult> GetSchedule(int serviceProviderId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAppointmentsByDateRangeAsync(serviceProviderId, startDate, endDate, cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("check-availability")]
    public async Task<IActionResult> CheckAvailability(
        [FromQuery] int serviceProviderId, 
        [FromQuery] DateTime appointmentDateTime, 
        [FromQuery] int durationInMinutes, 
        CancellationToken cancellationToken)
    {
        var isAvailable = await _appointmentService.IsTimeSlotAvailableAsync(serviceProviderId, appointmentDateTime, durationInMinutes, cancellationToken);
        return Ok(new { IsAvailable = isAvailable });
    }
}

public class CancelAppointmentRequest
{
    public int UserId { get; set; }
    public string Reason { get; set; } = string.Empty;
}