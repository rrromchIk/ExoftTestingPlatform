﻿using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register/admin")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserSignUpDto userSignUpDto)
    {
        var response = await _authService.RegisterAsync(userSignUpDto, "Admin");
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
}