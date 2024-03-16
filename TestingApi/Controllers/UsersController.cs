using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto;
using TestingApi.Dto.UserDto;
using TestingApi.Helpers;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFileService _fileService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger, IFileService fileService)
    {
        _userService = userService;
        _logger = logger;
        _fileService = fileService;
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpGet]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<UserResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] FiltersDto filtersDto,
        CancellationToken cancellationToken
    )
    {
        var response = await _userService.GetAllUsersAsync(filtersDto, cancellationToken);

        if (response.Items.IsNullOrEmpty())
            return NotFound();

        return Ok(response);
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
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

    [AllowAnonymous]
    [HttpPost("register")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
    public async Task<IActionResult> RegisterUser([FromBody] UserSignUpDto userSignUpDto,
        CancellationToken cancellationToken)
    {
        var response = await _userService.RegisterUserAsync(userSignUpDto, cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register/admin")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserSignUpDto userSignUpDto,
        CancellationToken cancellationToken)
    {
        var response = await _userService.RegisterUserAsync(userSignUpDto, cancellationToken, true);

        return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpPut("{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid id,
        [FromBody] UserUpdateDto userUpdateDto,
        CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(id, cancellationToken))
            return NotFound();

        await _userService.UpdateUserAsync(id, userUpdateDto, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [ValidateModel]
    [HttpPatch("{id:guid}/avatar")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserAvatar(
        [FromRoute] Guid id,
        [FromForm] [AllowedImageExtensions] IFormFile profilePicture,
        CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(id, cancellationToken))
            return NotFound();

        if (profilePicture.Length <= 0)
            return BadRequest("No file uploaded.");

        await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
        var filePath = await _fileService.StoreFileAsync(
            profilePicture,
            fileName: id.ToString(),
            cancellationToken
        );

        await _userService.UpdateUserAvatarAsync(id, filePath, cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpGet("{id:guid}/avatar/download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadUserAvatar(Guid id, CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(id, cancellationToken))
            return NotFound();

        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        var filePath = user.ProfilePictureFilePath;

        if (filePath.IsNullOrEmpty())
            return NotFound();

        var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
        return File(fileDto.Content, fileDto.MimeType, fileDto.FileName);
    }


    [Authorize(Roles = "SuperAdmin, Admin")]
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