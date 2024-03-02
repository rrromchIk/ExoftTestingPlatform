using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.UserAnswerDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[Route("api/users/")]
[ApiController]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAnswerDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAnswer(
        [FromRoute] Guid userId,
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken
    )
    {
        var response = await _userAnswerService.GetUserAnswerAsync(userId, questionId, cancellationToken);

        return Ok(response);
    }

    [HttpPost("answers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateUserAnswer([FromBody] UserAnswerDto userAnswerDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await _userService.UserExistsAsync(userAnswerDto.UserId ?? Guid.Empty, cancellationToken) &&
              await _questionService.QuestionExistsAsync(userAnswerDto.QuestionId ?? Guid.Empty, cancellationToken) &&
              await _answerService.AnswerExistsAsync(userAnswerDto.AnswerId ?? Guid.Empty, cancellationToken)))
            return NotFound();


        var response = await _userAnswerService.CreateUserAnswerAsync(userAnswerDto, cancellationToken);

        return CreatedAtAction(
            nameof(GetUserAnswer),
            new { userId = response.UserId, questionId = response.QuestionId },
            response
        );
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