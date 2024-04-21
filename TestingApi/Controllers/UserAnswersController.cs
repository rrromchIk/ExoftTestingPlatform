using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto.UserAnswerDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[Route("api/users/")]
[ApiController]
[Authorize(Roles = "SuperAdmin, Admin, User")]
public class UserAnswersController : ControllerBase
{
    private readonly IUserAnswerService _userAnswerService;
    private readonly IUserService _userService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerService _answerService;

    public UserAnswersController(IUserAnswerService userAnswerService, IUserService userService,
        IQuestionService questionService, IAnswerService answerService)
    {
        _userAnswerService = userAnswerService;
        _userService = userService;
        _questionService = questionService;
        _answerService = answerService;
    }

    [HttpGet("{userId:guid}/questions/{questionId:guid}/answers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<UserAnswerResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAnswers(
        [FromRoute] Guid userId,
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken
    )
    {
        var response = await _userAnswerService.GetUserAnswersAsync(
            userId,
            questionId,
            cancellationToken
        );
        return response.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpPost("answers")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateUserAnswer([FromBody] ICollection<UserAnswerDto> userAnswersDto,
        CancellationToken cancellationToken)
    {
        await _userAnswerService.CreateUserAnswersAsync(userAnswersDto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("{userId:guid}/questions/{questionId:guid}/answers/{answerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAnswer(
        [FromRoute] Guid userId,
        [FromRoute] Guid questionId,
        [FromRoute] Guid answerId,
        CancellationToken cancellationToken
    )
    {
        if (!await _userAnswerService.UserAnswerExistAsync(userId, questionId, answerId, cancellationToken))
            return NotFound();

        await _userAnswerService.DeleteUserAnswerAsync(userId, questionId, answerId, cancellationToken);
        return NoContent();
    }
}