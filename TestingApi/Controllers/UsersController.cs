using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.UserDto;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions; 

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<UserResponseDto>))]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] UserFiltersDto userFiltersDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var users = await _userService.GetAllUsersAsync(userFiltersDto, cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _userService.CreateUserAsync(userDto, cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        if (!await _userService.UserExistsAsync(id))
            return NotFound();

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}