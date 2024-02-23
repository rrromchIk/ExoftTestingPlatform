using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.Request;
using TestingApi.Dto.Response;
using TestingApi.Helpers;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests")]
public class TestsController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly ILogger<TestsController> _logger;

    public TestsController(ITestService testService, ILogger<TestsController> logger)
    {
        _testService = testService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TestResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetAllTests([FromQuery] TestFiltersDto testFiltersDto)
    {
        _logger.LogInformation("{dt}. Getting all tests. Filters applied: {f}",
            DateTime.Now.ToString(), JsonSerializer.Serialize(testFiltersDto));
        
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var response = await _testService.GetAllTestsAsync(testFiltersDto);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestById([FromRoute] Guid id)
    {
        _logger.LogInformation("{dt}. Getting test with id: {id}",
            DateTime.Now.ToString(), id);

        if (!await _testService.TestExistsAsync(id))
            return NotFound();

        var response = await _testService.GetTestByIdAsync(id);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestResponseDto))]
    public async Task<IActionResult> CreateTest([FromBody] TestDto testDto)
    {
        _logger.LogInformation("{dt}. Creating test: {dto}",
            DateTime.Now.ToString(), JsonSerializer.Serialize(testDto));

        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        

        var response = await _testService.CreateTestAsync(testDto);

        return CreatedAtAction(nameof(GetTestById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateTest([FromRoute] Guid id, [FromBody] TestDto testDto)
    {
        _logger.LogInformation("{dt}. Updating test with id: {id}. New test info: {dto}",
            DateTime.Now.ToString(), id, JsonSerializer.Serialize(testDto));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _testService.TestExistsAsync(id))
            return NotFound();
        
        if (!await _testService.UpdateTestAsync(id, testDto))
        {
            throw new DataException("Something went wrong while updating");
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTest([FromRoute] Guid id)
    {
        _logger.LogInformation("{DT}. Deleting test with id: {id}", 
            DateTime.Now.ToString(), id);
        
        if (!await _testService.TestExistsAsync(id))
            return NotFound();

        if (!await _testService.DeleteTestAsync(id))
        {
            throw new DataException("Something went wrong while deleting");
        }

        return NoContent();
    }
}