using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto;
using TestingApi.Dto.Response;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[ApiController]
[Route("api/tests")]
public class TestsController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly ILogger<TestsController> _logger;

    public TestsController(ITestService testService, IMapper mapper, ILogger<TestsController> logger) {
        _testService = testService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TestResponseDto>))]
    public async Task<IActionResult> GetAllTests()
    {
        var response = await _testService.GetAllTestsAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestById([FromRoute] Guid id) {
        if (!await _testService.ExistsAsync(id)) 
            return NotFound();

        var response = await _testService.GetByIdAsync(id);
        return Ok(response);
    }
}