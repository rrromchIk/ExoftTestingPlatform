using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto;
using TestingApi.Dto.TestDto;
using TestingApi.Helpers;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests")]
[Authorize(Roles = "SuperAdmin, Admin")]
public class TestsController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly ITestTmplService _testTmplService;
    private readonly ILogger<TestsController> _logger;

    public TestsController(ITestService testService, ILogger<TestsController> logger, ITestTmplService testTmplService)
    {
        _testService = testService;
        _logger = logger;
        _testTmplService = testTmplService;
    }
    
    [HttpGet]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTests([FromQuery] FiltersDto filtersDto,
        CancellationToken cancellationToken)
    {
        var response = await _testService.GetAllTestsAsync(filtersDto, cancellationToken);
        return response.Items.IsNullOrEmpty() ? NotFound() : Ok(response);
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testService.GetTestByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpGet("{id:guid}/questions-pools")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestWithQuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestWithQuestionsPoolsById([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _testService.GetTestWithQuestionsPoolsByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestWithQuestionsPoolResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTest([FromBody] TestWithQuestionsPoolsDto testWithQuestionsPoolsDto,
        CancellationToken cancellationToken)
    {
        var templateId = testWithQuestionsPoolsDto.TemplateId;
        if (templateId != null && await _testTmplService.TestTmplExistsAsync(
                templateId.GetValueOrDefault(),
                cancellationToken
            ))
            return NotFound("Template with such id not found");
        
        var response = await _testService.CreateTestAsync(testWithQuestionsPoolsDto, cancellationToken);
        return CreatedAtAction(nameof(GetTestById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid id,
        [FromBody] TestUpdateDto testUpdateDto,
        CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(id, cancellationToken))
            return NotFound();

        await _testService.UpdateTestAsync(id, testUpdateDto, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePublishedStatusForTest(
        [FromRoute] Guid id,
        [FromQuery] bool isPublished,
        CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(id, cancellationToken))
            return NotFound();

        await _testService.UpdateIsPublishedAsync(id, isPublished, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTest([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(id, cancellationToken))
            return NotFound();

        await _testService.DeleteTestAsync(id, cancellationToken);
        return NoContent();
    }
}