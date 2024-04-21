using TestingApi.Dto.StatisticDto;

namespace TestingApi.Services.Abstractions;

public interface IUserStatisticService
{
    Task<UserStatisticResponseDto> GetUserStatistic(Guid userId, CancellationToken cancellationToken = default);
    Task<float> GetUserPercentileRankForTheTest(Guid userId, Guid testId, CancellationToken cancellationToken = default);
}