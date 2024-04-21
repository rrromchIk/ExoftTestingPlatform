using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto.UserQuestionDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/users/")]
[Authorize(Roles = "SuperAdmin, Admin, User")]
public class UserQuestionsController : ControllerBase
{
    private readonly IUserQuestionService _userQuestionService;
    private readonly IUserTestService _userTestService;
    private readonly ITestService _testService;

    public UserQuestionsController(IUserQuestionService userQuestionService, IUserTestService userTestService, ITestService testService)
    {
        _userQuestionService = userQuestionService;
        _userTestService = userTestService;
        _testService = testService;
    }
    
    [HttpGet("tests/{testId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<QuestionsPoolDetailsDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllQuestionsForTestAsync(
        [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(testId, cancellationToken))
            return NotFound("Test with such id not found");
            
        var response = await _userQuestionService
            .GetAllQuestionsForTestAsync(testId, cancellationToken);

        return response.IsNullOrEmpty() ? NotFound() : Ok(response);
    }
    
    [HttpGet("{userId:guid}/tests/{testId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<UserQuestionDetailsResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserQuestions(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        var response = await _userQuestionService.GetUserQuestions(userId, testId, cancellationToken);

        return response.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpPost("questions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserQuestions([FromBody] ICollection<UserQuestionDto> userQuestionsDto,
        CancellationToken cancellationToken)
    {
        await _userQuestionService.CreateUserQuestions(userQuestionsDto, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }
    
}