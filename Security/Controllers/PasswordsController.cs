using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Helpers.ValidationAttributes;
using Security.Service.Abstractions;

namespace Security.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth/password")]
public class PasswordsController : ControllerBase
{
    private readonly IAuthService _authService;

    public PasswordsController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [ValidateModel]
    [HttpPost("forgot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        await _authService.ForgotPassword(forgotPasswordDto);
        return Ok();
    }

    [HttpGet("reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string token)
    {
        var response = new ResetPasswordDto
        {
            UserId = userId,
            Token = token
        };
        return Ok(response);
    }

    [ValidateModel]
    [HttpPost("reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        await _authService.ResetPassword(resetPasswordDto);
        return Ok();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [ValidateModel]
    [HttpPost("change")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        await _authService.ChangePassword(changePasswordDto);
        return Ok();
    }
}