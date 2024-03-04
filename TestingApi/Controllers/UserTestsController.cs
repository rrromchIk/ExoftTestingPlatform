using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/users/{userId:guid}/tests")]
public class UserTestsController : ControllerBase
{
    private readonly IUserTestService _userTestService;
    private readonly IUserService _userService;
    private readonly ITestService _testService;

    public UserTestsController(IUserTestService userTestService, IUserService userService, ITestService testService)
    {
        _userTestService = userTestService;
        _userService = userService;
        _testService = testService;
    }

    [HttpGet("{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserTestResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserTest(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken
    )
    {
        var response = await _userTestService.GetUserTestAsync(userId, testId, cancellationToken);

        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestToPassResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTestsForUser(
        [FromQuery] TestFiltersDto testFiltersDto,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken) {
        if (!await _userService.UserExistsAsync(userId, cancellationToken))
            return NotFound();

        var response = await _userTestService
            .GetAllTestsForUserAsync(testFiltersDto, userId, cancellationToken);

        if (response.Items.IsNullOrEmpty())
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("{testId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<TestPassingQuestionsPoolResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllQuestionsForUserTestAsync(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken) {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        var response = await _userTestService
            .GetQuestionsForUserTest(userId, testId, cancellationToken);
        
        if (response.IsNullOrEmpty())
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("started")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<StartedTestResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllStartedTestsForUserAsync(
        [FromQuery] TestFiltersDto testFiltersDto,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken) {
        if (!await _userService.UserExistsAsync(userId, cancellationToken))
            return NotFound();

        var response = await _userTestService
            .GetAllStartedTestsForUserAsync(testFiltersDto, userId, cancellationToken);
        
        if (response.Items.IsNullOrEmpty())
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpPost("{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateUserTest(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        if (!(await _userService.UserExistsAsync(userId, cancellationToken) &&
              await _testService.TestExistsAsync(testId, cancellationToken)))
            return NotFound();

        if (await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return BadRequest();


        var response = await _userTestService.CreateUserTestAsync(userId, testId, cancellationToken);

        return CreatedAtAction(
            nameof(GetUserTest),
            new { userId = response.UserId, testId = response.TestId },
            response
        );
    }
    
    [HttpPatch("{testId:guid}/complete/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteUserTest([FromRoute] Guid userId, [FromRoute] Guid testId, CancellationToken cancellationToken) {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        await _userTestService.CompleteUserTestAsync(userId, testId, cancellationToken);
            
        return Ok("Test completed successfully");
    }
    
    [HttpDelete("{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserTest(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken
    )
    {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        await _userTestService.DeleteUserTestAsync(userId, testId, cancellationToken);
        return NoContent();
    }
}