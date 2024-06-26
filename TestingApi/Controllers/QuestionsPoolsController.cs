﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/questions-pools/{id:guid}")]
[Authorize(Roles = "SuperAdmin, Admin")]
public class QuestionsPoolsController : ControllerBase
{
    private readonly IQuestionsPoolService _questionsPoolService;
    private readonly ITestService _testService;
    private readonly IQuestionsPoolTmplService _questionsPoolTmplService;
    private readonly ILogger<QuestionsPoolsController> _logger;

    public QuestionsPoolsController(IQuestionsPoolService questionsPoolService,
        ILogger<QuestionsPoolsController> logger,
        ITestService testService, IQuestionsPoolTmplService questionsPoolTmplService)
    {
        _questionsPoolService = questionsPoolService;
        _logger = logger;
        _testService = testService;
        _questionsPoolTmplService = questionsPoolTmplService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsPoolById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _questionsPoolService.GetQuestionPoolByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    
    [HttpPost("/api/tests/{testId:guid}/questions-pools")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(QuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateQuestionsPool(
        [FromRoute] Guid testId,
        [FromBody] QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(testId, cancellationToken))
            return NotFound("Test with such id not found");
        
        var templateId = questionsPoolDto.TemplateId;
        if (templateId != null && !await _questionsPoolTmplService.QuestionsPoolTmplExistsAsync(
                templateId.GetValueOrDefault(),
                cancellationToken
            ))
            return NotFound("Template with such id not found");
        
        var response = await _questionsPoolService.CreateQuestionsPoolAsync(
            testId,
            questionsPoolDto,
            cancellationToken
        );

        return CreatedAtAction(nameof(GetQuestionsPoolById), new { id = response.Id }, response);
    }

    [HttpPut]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateQuestionsPool(
        [FromRoute] Guid id,
        [FromBody] QuestionsPoolUpdateDto questionsPoolUpdateDto,
        CancellationToken cancellationToken)
    {
        if (!await _questionsPoolService.QuestionsPoolExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolService.UpdateQuestionsPoolAsync(id, questionsPoolUpdateDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteQuestionsPool([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _questionsPoolService.QuestionsPoolExistsAsync(id, cancellationToken))
            return NotFound();

        await _questionsPoolService.DeleteQuestionsPoolAsync(id, cancellationToken);
        return NoContent();
    }
}