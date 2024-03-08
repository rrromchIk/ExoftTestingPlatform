using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Dto;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.TestTemplateDto;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests/templates")]
public class TestTemplatesController : ControllerBase
{
    private readonly ITestTemplateService _testTemplateService;
    private readonly ILogger<TestTemplatesController> _logger;

    public TestTemplatesController(ILogger<TestTemplatesController> logger, ITestTemplateService testTemplateService)
    {
        _testTemplateService = testTemplateService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestTemplateResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetAllTestTemplates([FromQuery] FiltersDto filtersDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _testTemplateService.GetAllTestsTemplatesAsync(filtersDto, cancellationToken);

        if (response.Items.IsNullOrEmpty())
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestTemplateResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestTemplateById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testTemplateService.GetTestTemplateByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}/questions-pools/templates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestTemplateWithQpTemplatesResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestTemplateWithQpTemplatesById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testTemplateService.GetTestTemplateWithQuestionsPoolsTemplatesByIdAsync(id, cancellationToken);

        if (response == null) 
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestTemplateWithQpTemplatesResponseDto))]
    public async Task<IActionResult> CreateTestTemplate(
        [FromBody] TestTemplateWithQpTemplateDto testTemplateWithQpTemplateDto,
        CancellationToken cancellationToken)
    {
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _testTemplateService.CreateTestTemplateAsync(
            testTemplateWithQpTemplateDto, cancellationToken);

        return CreatedAtAction(nameof(GetTestTemplateWithQpTemplatesById), new { id = response.Id }, response);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateTest(
        [FromRoute] Guid id,
        [FromBody] TestTemplateDto testTemplateDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _testTemplateService.TestTemplateExistsAsync(id, cancellationToken))
            return NotFound();

        await _testTemplateService.UpdateTestTemplateAsync(id, testTemplateDto, cancellationToken);
        return NoContent();
    }

    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTestTemplate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!await _testTemplateService.TestTemplateExistsAsync(id, cancellationToken))
            return NotFound(); 

        await _testTemplateService.DeleteTestTemplateAsync(id, cancellationToken);
        return NoContent();
    }
}
