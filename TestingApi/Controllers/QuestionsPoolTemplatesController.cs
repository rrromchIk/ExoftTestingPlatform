using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/templates/questions-pools/templates/{id:guid}")]
public class QuestionsPoolTemplatesController : ControllerBase
{
    private readonly IQuestionsPoolTemplateService _questionsPoolTemplateService;
    private readonly ITestTemplateService _testTemplateService;
    private readonly ILogger<QuestionsPoolTemplatesController> _logger;

    public QuestionsPoolTemplatesController(ILogger<QuestionsPoolTemplatesController> logger,
        ITestTemplateService testTemplateService, IQuestionsPoolTemplateService questionsPoolTemplateService)
    {
        _logger = logger;
        _testTemplateService = testTemplateService;
        _questionsPoolTemplateService = questionsPoolTemplateService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionsPoolTemplateResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsPoolTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionsPoolTemplateService.GetQuestionPoolTemplateByIdAsync(id, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }
    
    [HttpPost("/api/tests/templates/{testTemplateId:guid}/questions-pools/templates")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionsPoolTemplateResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateQuestionsPoolTemplate(
        [FromRoute] Guid testTemplateId,
        [FromBody] QuestionsPoolTemplateDto questionsPoolTemplateDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!await _testTemplateService.TestTemplateExistsAsync(testTemplateId, cancellationToken))
            return NotFound();
        
        var response = await _questionsPoolTemplateService.CreateQuestionsPoolTemplateAsync(testTemplateId,
            questionsPoolTemplateDto,
            cancellationToken);

        return CreatedAtAction(nameof(GetQuestionsPoolTemplateById), new { id = response.Id }, response);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestionsPoolTemplate(
        [FromRoute] Guid id,
        [FromBody] QuestionsPoolTemplateDto questionsPoolTemplateDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _questionsPoolTemplateService.QuestionsPoolTemplateExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolTemplateService.UpdateQuestionsPoolTemplateAsync(id, questionsPoolTemplateDto, cancellationToken);
        return NoContent();
    } 

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestionsPoolTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionsPoolTemplateService.QuestionsPoolTemplateExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolTemplateService.DeleteQuestionsPoolTemplateAsync(id, cancellationToken);
        return NoContent();
    }
}