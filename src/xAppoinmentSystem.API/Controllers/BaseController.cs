using Microsoft.AspNetCore.Mvc;
using xAppoinmentSystem.API.Models;
using xAppoinmentSystem.Application.Models;

namespace xAppoinmentSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected IActionResult BaseResponse<T>(ServiceResult<T?> result)
    {
        if (result.Data == null)
        {
            return NotFound();
        }

        var response = ApiResponse<T>.From(result.Data);
        
        return Ok(response);
    }

    protected IActionResult BaseResponse(ServiceResultMessage result)
    {
        return BadRequest(result.Message);
    }

    protected IActionResult BaseResponse(ServiceResult result)
    {
        return NoContent();
    }
}