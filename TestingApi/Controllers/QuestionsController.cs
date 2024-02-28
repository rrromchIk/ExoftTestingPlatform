using System.Data;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.QuestionDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/questions-pools/")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionsPoolService _questionsPoolService;
    private readonly IQuestionService _questionService;
    private readonly ILogger<TestsController> _logger;

    public QuestionsController(IQuestionService questionService, ILogger<TestsController> logger, IQuestionsPoolService questionsPoolService)
    {
        _questionService = questionService;
        _logger = logger;
        _questionsPoolService = questionsPoolService;
    }
    
    [HttpGet("{questionsPoolId:guid}/questions")]
    public async Task<IActionResult> GetQuestionsByQuestionsPoolId([FromRoute] Guid questionsPoolId,
        CancellationToken cancellationToken)
    {
        var response = await _questionService
            .GetQuestionsByQuestionsPoolIdAsync(questionsPoolId, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{dt}. Getting test with id: {id}",
            DateTime.Now.ToString(),
            id
        );
        
        var response = await _questionService.GetQuestionByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpPost("{questionsPoolId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionResponseDto))]
    public async Task<IActionResult> CreateQuestion(
        [FromRoute] Guid questionsPoolId,
        [FromBody] QuestionWithAnswersDto questionWithAnswersDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _questionsPoolService.QuestionsPoolExistsAsync(questionsPoolId, cancellationToken))
        {
            return NotFound();
        }
        
        var response = await _questionService.CreateQuestionAsync(questionsPoolId, questionWithAnswersDto, cancellationToken);

        return CreatedAtAction(nameof(GetQuestionById), new { id = response.Id }, response);
    }
    
    [HttpPut("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] Guid id,
        [FromBody] QuestionDto questionDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _questionService.QuestionExistsAsync(id, cancellationToken))
            return NotFound();

        if (!await _questionService.UpdateQuestionAsync(id, questionDto, cancellationToken))
        {
            throw new DataException("Something went wrong while updating");
        }

        return NoContent();
    }

    [HttpDelete("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionService.QuestionExistsAsync(id, cancellationToken))
            return NotFound(); 

        if (!await _questionService.DeleteQuestionAsync(id, cancellationToken))
        {
            throw new DataException("Something went wrong while deleting");
        }

        return NoContent();
    }
}