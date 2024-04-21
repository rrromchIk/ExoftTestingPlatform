using Microsoft.AspNetCore.Mvc;
using TestingApi.Dto.StatisticDto;
using TestingApi.Services.Abstractions;

namespace TestingApi.Controllers;

[Route("api/statistic")]
[ApiController]
public class StatisticsController : Controller
{
    private readonly IUserStatisticService _userStatisticService;
    private readonly ITestStatisticService _testStatisticService;
    private readonly ITestService _testService;
    private readonly IUserService _userService;
    private readonly IUserTestService _userTestService;

    public StatisticsController(IUserStatisticService userStatisticService, ITestStatisticService testStatisticService,
        ITestService testService, IUserService userService, IUserTestService userTestService)
    {
        _userStatisticService = userStatisticService;
        _testStatisticService = testStatisticService;
        _testService = testService;
        _userService = userService;
        _userTestService = userTestService;
    }

    [HttpGet("tests/{testId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestStatisticResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestStatistic([FromRoute] Guid testId,
        CancellationToken cancellationToken)
    {
        if (!await _testService.TestExistsAsync(testId, cancellationToken))
            return NotFound();

        var response = await _testStatisticService.GetTestStatistic(testId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("users/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserStatisticResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserStatistic([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        if (!await _userService.UserExistsAsync(userId, cancellationToken))
            return NotFound();

        var response = await _userStatisticService.GetUserStatistic(userId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("users/{userId:guid}/tests/{testId:guid}/percentile-rank")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserStatisticResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserTestPercentileRank(
        [FromRoute] Guid userId,
        [FromRoute] Guid testId,
        CancellationToken cancellationToken
    )
    {
        if (!await _userTestService.UserTestExistsAsync(userId, testId, cancellationToken))
            return NotFound();

        var response = await _userStatisticService.GetUserPercentileRankForTheTest(userId, testId, cancellationToken);
        return Ok(response);
    }
}