using Microsoft.AspNetCore.Mvc;
using xAppoinmentSystem.Application.DTOs.Requests;
using xAppoinmentSystem.Application.Services;

namespace xAppoinmentSystem.API.Controllers;

public class BusinessController : BaseController
{
    private readonly IBusinessService _businessService;

    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBusiness(int id, CancellationToken cancellationToken)
    {
        var business = await _businessService.GetBusinessByIdAsync(id, cancellationToken);
        if (business == null) return NotFound();
        
        return Ok(business);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessRequest request, CancellationToken cancellationToken)
    {
        var business = await _businessService.CreateBusinessAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBusiness), new { id = business.Id }, business);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBusiness(int id, [FromBody] CreateBusinessRequest request, CancellationToken cancellationToken)
    {
        var business = await _businessService.UpdateBusinessAsync(id, request, cancellationToken);
        if (business == null) return NotFound();
        
        return Ok(business);
    }

    [HttpGet("by-location")]
    public async Task<IActionResult> GetBusinessesByLocation(
        [FromQuery] double latitude, 
        [FromQuery] double longitude, 
        [FromQuery] double radiusKm = 10, 
        CancellationToken cancellationToken = default)
    {
        var businesses = await _businessService.GetBusinessesByLocationAsync(latitude, longitude, radiusKm, cancellationToken);
        return Ok(businesses);
    }

    [HttpPost("{businessId}/request-ownership")]
    public async Task<IActionResult> RequestOwnership(int businessId, [FromBody] RequestOwnershipRequest request, CancellationToken cancellationToken)
    {
        var result = await _businessService.RequestBusinessOwnershipAsync(businessId, request.ServiceProviderId, cancellationToken);
        if (!result) return BadRequest("Ownership request already exists or failed");
        
        return Ok(new { Message = "Ownership request submitted successfully" });
    }

    [HttpPut("ownership/{ownershipId}/approve")]
    public async Task<IActionResult> ApproveOwnership(int ownershipId, [FromBody] ApproveOwnershipRequest request, CancellationToken cancellationToken)
    {
        var result = await _businessService.ApproveBusinessOwnershipAsync(ownershipId, request.OwnerId, cancellationToken);
        if (!result) return BadRequest("Ownership approval failed");
        
        return Ok(new { Message = "Ownership approved successfully" });
    }
}

public class RequestOwnershipRequest
{
    public int ServiceProviderId { get; set; }
}

public class ApproveOwnershipRequest
{
    public int OwnerId { get; set; }
}