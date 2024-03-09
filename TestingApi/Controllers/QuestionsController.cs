using System.Data;
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
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(IQuestionService questionService, ILogger<QuestionsController> logger, IQuestionsPoolService questionsPoolService)
    {
        _questionService = questionService;
        _logger = logger;
        _questionsPoolService = questionsPoolService;
    }
    
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

        if (response.IsNullOrEmpty()) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("questions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionService.GetQuestionByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
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
            return NotFound();
        
        var response = await _questionService.CreateQuestionAsync(questionsPoolId, questionWithAnswersDto, cancellationToken);

        return CreatedAtAction(nameof(GetQuestionById), new { id = response.Id }, response);
    }
    
    [HttpPut("questions/{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] Guid id,
        [FromBody] QuestionDto questionDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionService.QuestionExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionService.UpdateQuestionAsync(id, questionDto, cancellationToken);
        return NoContent();
    }

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