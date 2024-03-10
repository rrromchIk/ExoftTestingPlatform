using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto.QuestionTemplateDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/questions-pools/")]
[Authorize(Roles = "SuperAdmin, Admin, User")]
public class QuestionTemplatesController : ControllerBase
{
    private readonly IQuestionsPoolTmplService _questionsPoolTmplService;
    private readonly IQuestionTmplService _questionTmplService;
    private readonly ILogger<QuestionsController> _logger;
    
    public QuestionTemplatesController(IQuestionsPoolTmplService questionsPoolTmplService, IQuestionTmplService questionTmplService, ILogger<QuestionsController> logger)
    {
        _questionsPoolTmplService = questionsPoolTmplService;
        _questionTmplService = questionTmplService;
        _logger = logger;
    }
    
    [HttpGet("{questionsPoolTmplId:guid}/questions/templates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<QuestionTmplResponseDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionTemplatesByQuestionsPoolTmplId(
        [FromRoute] Guid questionsPoolTmplId, CancellationToken cancellationToken)
    {
        if (!await _questionsPoolTmplService.QuestionsPoolTmplExistsAsync(questionsPoolTmplId, cancellationToken))
            return NotFound();
        
        var response = await _questionTmplService
            .GetQuestionTmplsByQuestionsPoolTmplIdAsync(questionsPoolTmplId, cancellationToken);

        if (response.IsNullOrEmpty()) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("questions/templates/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionTmplService.GetQuestionTmplByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpPost("{questionsPoolTmplId:guid}/questions/templates")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateQuestionTemplate(
        [FromRoute] Guid questionsPoolTmplId,
        [FromBody] QuestionTmplWithAnswerTmplDto questionTmplWithAnswerTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionsPoolTmplService.QuestionsPoolTmplExistsAsync(questionsPoolTmplId, cancellationToken))
            return NotFound();
        
        var response = await _questionTmplService.CreateQuestionTmplAsync(
            questionsPoolTmplId, questionTmplWithAnswerTmplDto, cancellationToken);

        return CreatedAtAction(nameof(GetQuestionTemplateById), new { id = response.Id }, response);
    }
    
    [HttpPut("questions/templates/{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestionTemplate(
        [FromRoute] Guid id,
        [FromBody] QuestionTmplDto questionTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionTmplService.QuestionTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionTmplService.UpdateQuestionTmplAsync(id, questionTmplDto, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("questions/templates/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestionTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionTmplService.QuestionTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionTmplService.DeleteQuestionTmplAsync(id, cancellationToken);
        return NoContent();
    }
}