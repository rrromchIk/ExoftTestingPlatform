using TestingApi.Dto.StatisticDto;

namespace TestingApi.Services.Abstractions;

public interface ITestStatisticService
{
    Task<TestStatisticResponseDto> GetTestStatistic(Guid testId, CancellationToken cancellationToken = default);
}