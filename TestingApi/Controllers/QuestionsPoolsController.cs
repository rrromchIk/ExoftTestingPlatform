using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.Request;
using TestingApi.Dto.Response;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/")]
public class QuestionsPoolsController : ControllerBase
{
    private readonly IQuestionsPoolService _questionsPoolService;
    private readonly ILogger<TestsController> _logger;

    public QuestionsPoolsController(IQuestionsPoolService questionsPoolService, ILogger<TestsController> logger)
    {
        _questionsPoolService = questionsPoolService;
        _logger = logger;
    }
    
    [HttpGet("{testId:guid}/questions-pools")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<QuestionsPoolResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetQuestionsPoolsByTestId([FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{dt}. Getting questions pools for test: {testId}",
            DateTime.Now.ToString(), testId
        );
        
        var response = await _questionsPoolService.GetQuestionPoolsByTestIdAsync(
            testId, cancellationToken);
        return Ok(response);
    }
    
    
    [HttpGet("questions-pools/{id:guid}")]
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


    [HttpPost("questions-pools")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateQuestionsPool([FromBody] QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{dt}. Creating questions pool: {dto}",
            DateTime.Now.ToString(),
            JsonSerializer.Serialize(questionsPoolDto)
        );
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var response = await _questionsPoolService.CreateQuestionsPoolAsync(questionsPoolDto,
            cancellationToken);

        return CreatedAtAction(nameof(GetQuestionsPoolById), new { id = response.Id }, response);
    }
    
    [HttpPut("questions-pools/{id:guid}")]
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

    [HttpDelete("questions-pools/{id:guid}")]
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