using Microsoft.AspNetCore.Mvc;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Services;

namespace xAppoinmentSystem.API.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("service-providers/{id}")]
    public async Task<IActionResult> GetServiceProvider(int id, CancellationToken cancellationToken)
    {
        var provider = await _userService.GetServiceProviderByIdAsync(id, cancellationToken);
        if (provider == null) return NotFound();
        
        return Ok(provider);
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomer(int id, CancellationToken cancellationToken)
    {
        var customer = await _userService.GetCustomerByIdAsync(id, cancellationToken);
        if (customer == null) return NotFound();
        
        return Ok(customer);
    }

    [HttpPost("service-providers")]
    public async Task<IActionResult> CreateServiceProvider([FromBody] CreateServiceProviderRequest request, CancellationToken cancellationToken)
    {
        var provider = await _userService.CreateServiceProviderAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetServiceProvider), new { id = provider.Id }, provider);
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _userService.CreateCustomerAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    [HttpGet("service-providers/by-location")]
    public async Task<IActionResult> GetServiceProvidersByLocation(
        [FromQuery] double latitude, 
        [FromQuery] double longitude, 
        [FromQuery] double radiusKm = 10, 
        CancellationToken cancellationToken = default)
    {
        var providers = await _userService.GetServiceProvidersByLocationAsync(latitude, longitude, radiusKm, cancellationToken);
        return Ok(providers);
    }
}