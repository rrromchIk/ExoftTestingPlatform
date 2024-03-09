using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/templates/questions-pools/templates/{id:guid}")]
public class QuestionsPoolTemplatesController : ControllerBase
{
    private readonly IQuestionsPoolTmplService _questionsPoolTmplService;
    private readonly ITestTmplService _testTmplService;
    private readonly ILogger<QuestionsPoolTemplatesController> _logger;

    public QuestionsPoolTemplatesController(ILogger<QuestionsPoolTemplatesController> logger,
        ITestTmplService testTmplService, IQuestionsPoolTmplService questionsPoolTmplService)
    {
        _logger = logger;
        _testTmplService = testTmplService;
        _questionsPoolTmplService = questionsPoolTmplService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionsPoolTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsPoolTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionsPoolTmplService.GetQuestionPoolTmplByIdAsync(id, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }
    
    [HttpPost("/api/tests/templates/{testTemplateId:guid}/questions-pools/templates")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionsPoolTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateQuestionsPoolTemplate(
        [FromRoute] Guid testTemplateId,
        [FromBody] QuestionsPoolTmplDto questionsPoolTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _testTmplService.TestTmplExistsAsync(testTemplateId, cancellationToken))
            return NotFound();
        
        var response = await _questionsPoolTmplService.CreateQuestionsPoolTmplAsync(testTemplateId,
            questionsPoolTmplDto,
            cancellationToken);

        return CreatedAtAction(nameof(GetQuestionsPoolTemplateById), new { id = response.Id }, response);
    }
    
    [HttpPut]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestionsPoolTemplate(
        [FromRoute] Guid id,
        [FromBody] QuestionsPoolTmplDto questionsPoolTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionsPoolTmplService.QuestionsPoolTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolTmplService.UpdateQuestionsPoolTmplAsync(id, questionsPoolTmplDto, cancellationToken);
        return NoContent();
    } 

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestionsPoolTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionsPoolTmplService.QuestionsPoolTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolTmplService.DeleteQuestionsPoolTmplAsync(id, cancellationToken);
        return NoContent();
    }
}