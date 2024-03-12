using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Helpers.ValidationAttributes;
using Security.Service.Abstractions;

namespace Security.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] UserSignUpDto userSignUpDto)
    {
        var response = await _authService.RegisterAsync(userSignUpDto);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var response = await _authService.LoginAsync(userLoginDto);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
    {
        var response = await _authService.RefreshAccessTokenAsync(tokenDto);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("email/verification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EmailVerification([FromQuery] Guid userId, [FromQuery] string token)
    {
        await _authService.VerifyEmail(userId, token);
        return Ok();
    }

    [AllowAnonymous]
    [ValidateModel]
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        await _authService.ForgotPassword(forgotPasswordDto);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string token)
    {
        var response = new ResetPasswordDto
        {
            UserId = userId,
            Token = token
        };
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        await _authService.ResetPassword(resetPasswordDto);
        return Ok();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpPost("change-password")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        await _authService.ChangePassword(changePasswordDto);
        return Ok();
    }
}