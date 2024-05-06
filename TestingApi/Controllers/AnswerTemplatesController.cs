using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.AnswerTemplateDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/tests/questions-pools/questions/")]
[Authorize(Roles = "SuperAdmin, Admin")]
public class AnswerTemplatesController : ControllerBase
{
    private readonly IAnswerTmplService _answerTmplService;
    private readonly IQuestionTmplService _questionTmplService;
    private readonly ILogger<AnswersController> _logger;

    //logger, questionService and answerService are not needed. also very long string, better to split into several lines
    public AnswerTemplatesController(IQuestionService questionService, ILogger<AnswersController> logger, IAnswerService answerService, IAnswerTmplService answerTmplService, IQuestionTmplService questionTmplService)
    {
        _logger = logger;
        _answerTmplService = answerTmplService;
        _questionTmplService = questionTmplService;
    }
    
    [HttpGet("answers/templates/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnswerTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnswerTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _answerTmplService.GetAnswerTmplById(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }
    
    [HttpPost("{questionTemplateId:guid}/answers/templates")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AnswerTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAnswerTemplate(
        [FromRoute] Guid questionTemplateId,
        [FromBody] AnswerTmplDto answerTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionTmplService.QuestionTmplExistsAsync(questionTemplateId, cancellationToken))
            return NotFound();
        
        var response = await _answerTmplService.CreateAnswerTmplAsync(questionTemplateId, answerTmplDto, cancellationToken);
        return CreatedAtAction(nameof(GetAnswerTemplateById), new { id = response.Id }, response);
    }
    
    [HttpPut("answers/templates/{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateAnswer(
        [FromRoute] Guid id,
        [FromBody] AnswerTmplDto answerTmplDto,
        CancellationToken cancellationToken) 
    {
        if (!await _answerTmplService.AnswerTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _answerTmplService.UpdateAnswerTmplAsync(id, answerTmplDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("answers/templates/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAnswerTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _answerTmplService.AnswerTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _answerTmplService.DeleteAnswerTmplAsync(id, cancellationToken);
        return NoContent();
    }
}