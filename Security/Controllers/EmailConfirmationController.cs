using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Helpers.ValidationAttributes;
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
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto emailConfirmationDto) 
    {
        await _authService.ConfirmEmail(emailConfirmationDto);
        return Ok();
    }
}