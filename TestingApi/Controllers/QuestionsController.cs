using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto.QuestionDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/questions-pools/")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsPoolService _questionsPoolService;
    private readonly IQuestionService _questionService;
    private readonly IQuestionTmplService _questionTmplService;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(IQuestionService questionService, ILogger<QuestionsController> logger,
        IQuestionsPoolService questionsPoolService, IQuestionTmplService questionTmplService)
    {
        _questionService = questionService;
        _logger = logger;
        _questionsPoolService = questionsPoolService;
        _questionTmplService = questionTmplService;
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpGet("{questionsPoolId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<QuestionResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsByQuestionsPoolId([FromRoute] Guid questionsPoolId,
        CancellationToken cancellationToken)
    {
        if (!await _questionsPoolService.QuestionsPoolExistsAsync(questionsPoolId, cancellationToken))
            return NotFound();

        var response = await _questionService
            .GetQuestionsByQuestionsPoolIdAsync(questionsPoolId, cancellationToken);

        return response.IsNullOrEmpty() ? NotFound() : Ok(response);
    }
    
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    [HttpGet("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionService.GetQuestionByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPost("{questionsPoolId:guid}/questions")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateQuestion(
        [FromRoute] Guid questionsPoolId,
        [FromBody] QuestionWithAnswersDto questionWithAnswersDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionsPoolService.QuestionsPoolExistsAsync(questionsPoolId, cancellationToken))
            return NotFound("Questions pool with such id not found");

        var templateId = questionWithAnswersDto.TemplateId;
        if (templateId != null && await _questionTmplService.QuestionTmplExistsAsync(
                templateId.GetValueOrDefault(),
                cancellationToken
            ))
            return NotFound("Template with such id not found");
        
        var response = await _questionService.CreateQuestionAsync(
            questionsPoolId,
            questionWithAnswersDto,
            cancellationToken
        );

        return CreatedAtAction(nameof(GetQuestionById), new { id = response.Id }, response);
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPut("questions/{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] Guid id,
        [FromBody] QuestionUpdateDto questionUpdateDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionService.QuestionExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionService.UpdateQuestionAsync(id, questionUpdateDto, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpDelete("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionService.QuestionExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionService.DeleteQuestionAsync(id, cancellationToken);
        return NoContent();
    }
}