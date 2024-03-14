using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto;
using TestingApi.Dto.TestTemplateDto;
using TestingApi.Helpers;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/templates")]
[Authorize(Roles = "SuperAdmin, Admin, User")]
public class TestTemplatesController : ControllerBase
{
    private readonly ITestTmplService _testTmplService;
    private readonly ILogger<TestTemplatesController> _logger;

    public TestTemplatesController(ILogger<TestTemplatesController> logger, ITestTmplService testTmplService)
    {
        _testTmplService = testTmplService;
        _logger = logger;
    }

    [HttpGet]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestTmplResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTestTemplates([FromQuery] FiltersDto filtersDto,
        CancellationToken cancellationToken)
    {
        var response = await _testTmplService.GetAllTestsTmplsAsync(filtersDto, cancellationToken);
        return response.Items.IsNullOrEmpty() ? NotFound() : Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestTmplResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testTmplService.GetTestTmplByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpGet("{id:guid}/questions-pools/templates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestTmplWithQpTmplsResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestTemplateWithQpTemplatesById([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _testTmplService.GetTestTmplWithQuestionsPoolsTmplByIdAsync(id, cancellationToken);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestTmplWithQpTmplsResponseDto))]
    public async Task<IActionResult> CreateTestTemplate(
        [FromBody] TestTmplWithQuestionsPoolTmplDto testTmplWithQuestionsPoolTmplDto,
        CancellationToken cancellationToken)
    {
        var response = await _testTmplService.CreateTestTmplAsync(
            testTmplWithQuestionsPoolTmplDto,
            cancellationToken
        );

        return CreatedAtAction(nameof(GetTestTemplateWithQpTemplatesById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid id,
        [FromBody] TestTmplDto testTmplDto,
        CancellationToken cancellationToken)
    {
        if (!await _testTmplService.TestTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _testTmplService.UpdateTestTmplAsync(id, testTmplDto, cancellationToken);
        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTestTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _testTmplService.TestTmplExistsAsync(id, cancellationToken))
            return NotFound();

        await _testTmplService.DeleteTestTmplAsync(id, cancellationToken);
        return NoContent();
    }
}