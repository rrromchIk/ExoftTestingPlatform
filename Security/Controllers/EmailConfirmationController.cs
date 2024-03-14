using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Service.Abstractions;

namespace Security.Controllers;

[ApiController]
[Route("api/auth/email/confirm")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IAuthService _authService;

    public EmailConfirmationController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpGet("request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ConfirmEmailRequest()
    {
        await _authService.ConfirmEmailRequest();
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        await _authService.ConfirmEmail(userId, token);
        return Ok();
    }
}