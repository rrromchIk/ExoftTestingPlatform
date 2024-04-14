﻿using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.StatisticDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserStatisticService : IUserStatisticService
{
    private readonly DataContext _dataContext;

    public UserStatisticService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<UserStatisticResponseDto> GetUserStatistic(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var allStartedTestsQuery = _dataContext.UserTests
            .Where(ut => ut.UserId == userId && ut.UserTestStatus != UserTestStatus.NotStarted);

        var allCompletedTestsQuery = allStartedTestsQuery
            .Where(ut => ut.UserTestStatus == UserTestStatus.Completed);

        var testResultsQuery = allCompletedTestsQuery
            .Select(
                ut => ut.UserScore / ut.TotalScore * 100
            );

        var amountOfStartedTests = await allStartedTestsQuery.CountAsync(cancellationToken);
        var amountOfTestsCompleted = await allCompletedTestsQuery
            .CountAsync(cancellationToken);

        var amountOfTestsInProcess = await _dataContext.UserTests
            .Where(ut => ut.UserId == userId && ut.UserTestStatus == UserTestStatus.InProcess)
            .CountAsync(cancellationToken);

        float? averagePercentageScore = await testResultsQuery.AnyAsync(cancellationToken)
            ? await testResultsQuery.AverageAsync(cancellationToken)
            : null;

        return new UserStatisticResponseDto
        {
            AmountOfStartedTests = amountOfStartedTests,
            AmountOfCompletedTests = amountOfTestsCompleted,
            AmountOfInProcessTests = amountOfTestsInProcess,
            AverageResult = averagePercentageScore,
            AllTestsResults = await testResultsQuery.ToListAsync(cancellationToken)
        };
    }

    public async Task<float> GetUserPercentileRankForTheTest(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var currentUserScoreInPercents = await _dataContext.UserTests.Where(
                ut => ut.UserId == userId &&
                      ut.TestId == testId
            )
            .Select(ut => ut.UserScore / ut.TotalScore * 100)
            .FirstAsync(cancellationToken);

        var allUsersScoresOnTest = await _dataContext.UserTests
            .Where(ut => ut.TestId == testId && ut.UserTestStatus == UserTestStatus.Completed)
            .Select(ut =>  ut.UserScore / ut.TotalScore * 100)
            .OrderBy(ut => ut)
            .Distinct()
            .ToListAsync(cancellationToken);

        var userIndex = allUsersScoresOnTest.FindIndex(
            userScore =>
                userScore.Equals(currentUserScoreInPercents)
        );

        if (allUsersScoresOnTest.Count - 1 == 0)
            return 0;

        var percentileRank = (float)(userIndex) / (allUsersScoresOnTest.Count - 1) * 100;
        return percentileRank;
    }
}