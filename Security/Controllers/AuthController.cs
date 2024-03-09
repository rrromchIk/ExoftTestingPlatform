using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Helpers.ValidationAttributes;
using Security.Service.Abstractions;

namespace Security.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserSignUpDto userSignUpDto)
    {
        var response = await _userService.CreateUserAsync(userSignUpDto);
        return Ok(response);
    }

    [HttpPost("login")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var response = await _userService.Login(userLoginDto);
        return Ok(response);
    }
}