using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserTestService : IUserTestService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserTestService> _logger;

    public UserTestService(DataContext dataContext, ILogger<UserTestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserTestResponseDto?> GetUserTestAsync(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var userTestFounded = await _dataContext.UserTests
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        return _mapper.Map<UserTestResponseDto>(userTestFounded);
    }

    public async Task<PagedList<TestToPassResponseDto>> GetAllTestsForUserAsync(TestFiltersDto testFiltersDto,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var testsQuery = _dataContext.Tests
            .Include(t => t.UserTests)
            .Select(
                t => new TestToPassResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Subject = t.Subject,
                    Duration = t.Duration,
                    CreatedTimestamp = t.CreatedTimestamp,
                    Difficulty = t.Difficulty.ToString(),
                    UserTestStatus = t.UserTests
                        .Where(ut => ut.UserId == userId)
                        .Select(ut => ut.UserTestStatus)
                        .FirstOrDefault()
                        .ToString()
                }
            );

        if (!string.IsNullOrWhiteSpace(testFiltersDto.SearchTerm))
        {
            testsQuery = testsQuery.Where(
                t =>
                    t.Name.Contains(testFiltersDto.SearchTerm) ||
                    t.Subject.Contains(testFiltersDto.SearchTerm)
            );
        }

        testsQuery = testFiltersDto.SortOrder?.ToLower() == "desc"
            ? testsQuery.OrderByDescending(GetSortPropertyForTestToPass(testFiltersDto.SortColumn))
            : testsQuery.OrderBy(GetSortPropertyForTestToPass(testFiltersDto.SortColumn));

        return await PagedList<TestToPassResponseDto>.CreateAsync(
            testsQuery,
            testFiltersDto.Page,
            testFiltersDto.PageSize,
            cancellationToken
        );
    }

    public async Task<PagedList<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(TestFiltersDto testFiltersDto,
        Guid userId, CancellationToken cancellationToken = default)
    {
        var testsQuery = _dataContext.UserTests
            .Include(ut => ut.Test)
            .Select(
                ut => new StartedTestResponseDto
                {
                    TotalScore = ut.TotalScore,
                    UserScore = ut.UserScore,
                    StartingTime = ut.StartingTime,
                    EndingTime = ut.EndingTime,
                    UserTestStatus = ut.UserTestStatus.ToString(),
                    Test = new TestResponseDto()
                    {
                        Name = ut.Test.Name,
                        Subject = ut.Test.Subject,
                        Duration = ut.Test.Duration,
                        IsPublished = ut.Test.IsPublished,
                        Difficulty = ut.Test.Difficulty.ToString(),
                    }
                }
            );

        if (!string.IsNullOrWhiteSpace(testFiltersDto.SearchTerm))
        {
            testsQuery = testsQuery.Where(
                t =>
                    t.Test.Name.Contains(testFiltersDto.SearchTerm) ||
                    t.Test.Subject.Contains(testFiltersDto.SearchTerm)
            );
        }

        testsQuery = testFiltersDto.SortOrder?.ToLower() == "desc"
            ? testsQuery.OrderByDescending(GetSortPropertyForStartedTest(testFiltersDto.SortColumn))
            : testsQuery.OrderBy(GetSortPropertyForStartedTest(testFiltersDto.SortColumn));

        return await PagedList<StartedTestResponseDto>.CreateAsync(
            testsQuery,
            testFiltersDto.Page,
            testFiltersDto.PageSize,
            cancellationToken
        );
    }

    public async Task<ICollection<TestPassingQuestionsPoolResponseDto>> GetQuestionsForUserTest(Guid userId,
        Guid testId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPools
            .Include(qp => qp.Questions)
            .Where(qp => qp.TestId == testId)
            .Select(
                qp => new TestPassingQuestionsPoolResponseDto
                {
                    QuestionsPoolId = qp.Id,
                    GenerationStrategy = qp.GenerationStrategy.ToString(),
                    NumOfQuestionsToBeGenerated = qp.NumOfQuestionsToBeGenerated,
                    UserQuestions = qp.Questions
                        .Select(
                            q => new UserQuestionResponseDto
                            {
                                QuestionId = q.Id,
                                IsAnswered = _dataContext.UserAnswers.Any(ua => ua.QuestionId == q.Id)
                            }
                        ).ToList()
                }
            ).ToListAsync(cancellationToken);
    }

    public async Task<bool> UserTestExistsAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserTests
            .AnyAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);
    }

    public async Task<UserTestResponseDto> CreateUserTestAsync(Guid userId, Guid testId,
        float totalScore, CancellationToken cancellationToken = default)
    {
        var userTestToAdd = new UserTest()
        {
            UserId = userId,
            TestId = testId,
            TotalScore = totalScore,
            StartingTime = DateTime.Now,
            UserTestStatus = UserTestStatus.InProcess
        };

        var addedUserTest = await _dataContext.AddAsync(userTestToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserTestResponseDto>(addedUserTest.Entity);
    }

    public async Task CompleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        var userTestToComplete = await _dataContext.UserTests
            .FirstAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        userTestToComplete.UserTestStatus = UserTestStatus.Completed;
        userTestToComplete.UserScore = await CalculateUserScore(userId, testId, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        var userTestToDelete = await _dataContext.UserTests
            .FirstAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        _dataContext.Remove(userTestToDelete);

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    private async ValueTask<float> CalculateUserScore(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var userAnswersDetailsQuery = _dataContext.UserAnswers
            .Include(ua => ua.Question)
            .ThenInclude(q => q.QuestionsPool)
            .Where(ua => ua.UserId == userId && ua.Question.QuestionsPool.TestId == testId)
            .SelectMany(
                ua => _dataContext.Answers
                    .Where(a => a.QuestionId == ua.QuestionId)
                    .Select(
                        a => new
                        {
                            ua.QuestionId,
                            AnswerId = a.Id,
                            ua.Question.MaxScore,
                            IsAnswerCorrect = a.IsCorrect,
                            UserAnsweredCorrect = a.IsCorrect && ua.AnswerId == a.Id,
                            UserAnsweredWrong = ua.AnswerId == a.Id && !a.IsCorrect
                        }
                    )
            )
            .GroupBy(userAnswers => new { userAnswers.QuestionId, userAnswers.AnswerId })
            .Select(
                group => new
                {
                    group.Key.QuestionId,
                    group.Key.AnswerId,
                    group.First().MaxScore,
                    group.First().IsAnswerCorrect,
                    UserAnsweredCorrect = group.Any(userAnswers => userAnswers.UserAnsweredCorrect),
                    UserAnsweredWrong = group.Any(userAnswers => userAnswers.UserAnsweredWrong)
                }
            )
            .GroupBy(userAnswers => userAnswers.QuestionId)
            .Select(
                userAnswersGroup => new
                {
                    userAnswersGroup.First().MaxScore,
                    TotalCorrectAnswersCount = userAnswersGroup.Count(userAnswers => userAnswers.IsAnswerCorrect),
                    UserCorrectAnswersCount = userAnswersGroup.Count(userAnswers => userAnswers.UserAnsweredCorrect),
                    UserWrongAnswersCount = userAnswersGroup.Count(userAnswers => userAnswers.UserAnsweredWrong)
                }
            );
        
        var userScore = await userAnswersDetailsQuery
            .Where(res => (res.UserCorrectAnswersCount - res.UserWrongAnswersCount) > 0)
            .SumAsync(
                res => 
                    (res.UserCorrectAnswersCount - res.UserWrongAnswersCount) * res.MaxScore / res.TotalCorrectAnswersCount,
                cancellationToken
            );
        
        return userScore;
    }

    private static Expression<Func<TestToPassResponseDto, object>> GetSortPropertyForTestToPass(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => t => t.Name,
            "subject" => t => t.Subject,
            "difficulty" => t => t.Difficulty,
            "duration" => t => t.Duration,
            "creationTime" => t => t.CreatedTimestamp,
            _ => t => t.Id
        };
    }

    private static Expression<Func<StartedTestResponseDto, object>> GetSortPropertyForStartedTest(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => t => t.Test.Name,
            "subject" => t => t.Test.Subject,
            "difficulty" => t => t.Test.Difficulty,
            "duration" => t => t.Test.Duration,
            "startingTime" => t => t.StartingTime,
            "endingTime" => t => t.EndingTime,
            _ => t => t.Test.Name
        };
    }
}