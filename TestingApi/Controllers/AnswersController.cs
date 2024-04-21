using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.AnswerDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/tests/questions-pools/questions/")]
[Authorize(Roles = "SuperAdmin, Admin")]
public class AnswersController : ControllerBase
{
    private readonly IAnswerService _answerService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerTmplService _answerTmplService;
    private readonly ILogger<AnswersController> _logger;

    public AnswersController(IQuestionService questionService, ILogger<AnswersController> logger,
        IAnswerService answerService, IAnswerTmplService answerTmplService)
    {
        _questionService = questionService;
        _logger = logger;
        _answerService = answerService;
        _answerTmplService = answerTmplService;
    }

    [HttpGet("answers/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnswerResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnswerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _answerService.GetAnswerById(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }


    [HttpPost("{questionId:guid}/answers")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AnswerResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAnswer(
        [FromRoute] Guid questionId,
        [FromBody] AnswerDto answerDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionService.QuestionExistsAsync(questionId, cancellationToken))
            return NotFound("Question with such id not found");
        
        var templateId = answerDto.TemplateId;
        if (templateId != null && !await _answerTmplService.AnswerTmplExistsAsync(
                templateId.GetValueOrDefault(),
                cancellationToken
            ))
            return NotFound("Template with such id not found");

        var response = await _answerService.CreateAnswerAsync(questionId, answerDto, cancellationToken);
        return CreatedAtAction(nameof(GetAnswerById), new { id = response.Id }, response);
    }


    [HttpPut("answers/{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateAnswer(
        [FromRoute] Guid id,
        [FromBody] AnswerUpdateDto answerUpdateDto,
        CancellationToken cancellationToken)
    {
        if (!await _answerService.AnswerExistsAsync(id, cancellationToken))
            return NotFound();

        await _answerService.UpdateAnswerAsync(id, answerUpdateDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("answers/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _answerService.AnswerExistsAsync(id, cancellationToken))
            return NotFound();

        await _answerService.DeleteAnswerAsync(id, cancellationToken);
        return NoContent();
    }
}