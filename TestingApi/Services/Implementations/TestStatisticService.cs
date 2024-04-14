using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.StatisticDto;
using TestingApi.Dto.TestDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class TestStatisticService : ITestStatisticService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public TestStatisticService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<TestStatisticResponseDto> GetTestStatistic(Guid testId,
        CancellationToken cancellationToken = default)
    {
        var test = await _dataContext.Tests.Where(t => t.Id == testId).FirstAsync(cancellationToken);
        var completedTestsQuery = _dataContext.UserTests
            .Where(ut => ut.TestId == testId && ut.UserTestStatus == UserTestStatus.Completed);
        
        var totalAmountOfAttemptsTaken = await completedTestsQuery
            .CountAsync(cancellationToken);
        
        var amountOfCurrentGoingAttempts = await _dataContext.UserTests
            .Where(ut => ut.TestId == testId && ut.UserTestStatus == UserTestStatus.InProcess)
            .CountAsync(cancellationToken);

        var averageUsersScore = await completedTestsQuery
            .Select(ut => ut.UserScore / ut.TotalScore * 100)
            .DefaultIfEmpty()
            .AverageAsync(cancellationToken);
        
        var averageUsersTimeSpent = completedTestsQuery
            .AsEnumerable()
            .Select(ut => (float)ut.EndingTime.Subtract(ut.StartingTime).TotalMinutes)
            .DefaultIfEmpty()
            .Average();
        
        var allUsersScores = await completedTestsQuery
            .Select(ut => ut.UserScore / ut.TotalScore * 100)
            .ToListAsync(cancellationToken);

        return new TestStatisticResponseDto
        {
            Test = _mapper.Map<TestResponseDto>(test),
            TotalAmountOfAttemptsTaken = totalAmountOfAttemptsTaken,
            AmountOfCurrentGoingAttempts = amountOfCurrentGoingAttempts,
            AverageUsersTimeSpent = averageUsersTimeSpent,
            AverageUsersScore = averageUsersScore,
            AllUsersScores = allUsersScores
        };
    }
}