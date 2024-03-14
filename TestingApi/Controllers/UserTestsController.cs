using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto;
using TestingApi.Dto.TestResultDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/users/{userId:guid}/tests")]
[Authorize(Roles = "SuperAdmin, Admin, Users")]
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
        return response == null ? NotFound() : Ok(response);
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestToPassResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTestsForUser(
        [FromQuery] FiltersDto filtersDto,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(userId, cancellationToken))
            return NotFound();

        var response = await _userTestService
            .GetAllTestsForUserAsync(filtersDto, userId, cancellationToken);

        return response.Items.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpGet("{testId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<TestPassingQuestionsPoolResponseDto>))]
    public async Task<IActionResult> GetAllQuestionsForUserTestAsync(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {

        var response = await _userTestService
            .GetQuestionsForUserTest(userId, testId, cancellationToken);

        return response.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpGet("started")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<StartedTestResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllStartedTestsForUserAsync(
        [FromQuery] FiltersDto filtersDto,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(userId, cancellationToken))
            return NotFound();

        var response = await _userTestService
            .GetAllStartedTestsForUserAsync(filtersDto, userId, cancellationToken);

        return response.Items.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpPost("{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateUserTest(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        [FromBody] float totalScore,
        CancellationToken cancellationToken)
    {
        if (!(await _userService.UserExistsAsync(userId, cancellationToken) &&
              await _testService.TestExistsAsync(testId, cancellationToken)))
            return NotFound();

        if (await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return BadRequest();


        var response = await _userTestService.CreateUserTestAsync(userId, testId, totalScore, cancellationToken);

        return CreatedAtAction(
            nameof(GetUserTest),
            new { userId = response.UserId, testId = response.TestId },
            response
        );
    }

    [HttpPatch("{testId:guid}/complete/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteUserTest([FromRoute] Guid userId, [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        await _userTestService.CompleteUserTestAsync(userId, testId, cancellationToken);

        var response = await _userTestService.GetUserTestAsync(userId, testId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{testId:guid}/results")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<TestResultResponseDto>))]
    public async Task<IActionResult> GetUserTestResult([FromRoute] Guid userId, [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        var response = await _userTestService.GetUserTestResults(userId, testId, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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