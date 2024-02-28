using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/questions-pools")]
public class QuestionsPoolsController : ControllerBase
{
    private readonly IQuestionsPoolService _questionsPoolService;
    private readonly ITestService _testService;
    private readonly ILogger<TestsController> _logger;

    public QuestionsPoolsController(IQuestionsPoolService questionsPoolService, ILogger<TestsController> logger,
        ITestService testService)
    {
        _questionsPoolService = questionsPoolService;
        _logger = logger;
        _testService = testService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsPoolById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{dt}. Getting questions pool with id: {id}",
            DateTime.Now.ToString(),
            id
        );

        var response = await _questionsPoolService.GetQuestionPoolByIdAsync(id, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestionsPool(
        [FromRoute] Guid id,
        [FromBody] QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{dt}. Updating questions pool with id: {id}. New questions pool info: {dto}",
            DateTime.Now.ToString(),
            id,
            JsonSerializer.Serialize(questionsPoolDto)
        );

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _questionsPoolService.QuestionsPoolExistsAsync(id, cancellationToken))
            return NotFound();

        if (!await _questionsPoolService.UpdateQuestionsPoolAsync(id, questionsPoolDto, cancellationToken))
        {
            throw new DataException("Something went wrong while updating");
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestionsPool([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{DT}. Deleting questions pool with id: {id}",
            DateTime.Now.ToString(),
            id
        );

        if (!await _questionsPoolService.QuestionsPoolExistsAsync(id, cancellationToken))
            return NotFound();

        if (!await _questionsPoolService.DeleteQuestionsPoolAsync(id, cancellationToken))
        {
            throw new DataException("Something went wrong while deleting");
        }

        return NoContent();
    }
}