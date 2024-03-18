using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.TestResultDto;
using TestingApi.Dto.UserTestDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Models.Test;
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

    public async Task<PagedList<TestToPassResponseDto>> GetAllTestsForUserAsync(FiltersDto filtersDto,
        Guid userId, CancellationToken cancellationToken = default)
    {
        IQueryable<Test> testsQuery = _dataContext.Tests
            .Where(t => t.IsPublished)
            .Include(t => t.UserTests);
        
        if (!string.IsNullOrWhiteSpace(filtersDto.SearchTerm))
        {
            testsQuery = testsQuery.Where(
                t =>
                    t.Name.Contains(filtersDto.SearchTerm) ||
                    t.Subject.Contains(filtersDto.SearchTerm)
            );
        }

        testsQuery = filtersDto.SortOrder?.ToLower() == "desc"
            ? testsQuery.OrderByDescending(GetSortPropertyForTestToPass(filtersDto.SortColumn))
            : testsQuery.OrderBy(GetSortPropertyForTestToPass(filtersDto.SortColumn));
        
        var testsToPassQuery = testsQuery
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

        return await PagedList<TestToPassResponseDto>.CreateAsync(
            testsToPassQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );
    }

    public async Task<PagedList<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(FiltersDto filtersDto,
        Guid userId, CancellationToken cancellationToken = default)
    {
        var testsQuery = _dataContext.UserTests
            .Include(ut => ut.Test)
            .Where(ut => ut.UserId == userId);
        
        if (!string.IsNullOrWhiteSpace(filtersDto.SearchTerm))
        {
            testsQuery = testsQuery.Where(
                t =>
                    t.Test.Name.Contains(filtersDto.SearchTerm) ||
                    t.Test.Subject.Contains(filtersDto.SearchTerm)
            );
        }

        testsQuery = filtersDto.SortOrder?.ToLower() == "desc"
            ? testsQuery.OrderByDescending(GetSortPropertyForStartedTest(filtersDto.SortColumn))
            : testsQuery.OrderBy(GetSortPropertyForStartedTest(filtersDto.SortColumn));

        var startedTestQuery = testsQuery
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
                        Id = ut.Test.Id,
                        Name = ut.Test.Name,
                        Subject = ut.Test.Subject,
                        Duration = ut.Test.Duration,
                        IsPublished = ut.Test.IsPublished,
                        Difficulty = ut.Test.Difficulty.ToString(),
                    }
                }
            );

        return await PagedList<StartedTestResponseDto>.CreateAsync(
            startedTestQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );
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
        var userQuestionsResultsQuery = GetAnsweredUserQuestionResultQuery(userId, testId);

        return (await userQuestionsResultsQuery.ToListAsync(cancellationToken))
            .Sum(q => q.UserScore);
    }


    public async Task<TestResultResponseDto> GetUserTestResults(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var userTest = await _dataContext.UserTests
            .Where(t => t.UserId == userId && t.TestId == testId)
            .FirstAsync(cancellationToken);
        
        if (userTest.UserTestStatus != UserTestStatus.Completed)
            throw new ApiException("Test is not completed", StatusCodes.Status400BadRequest);
            
        var answeredUserQuestionsResultsQuery = GetAnsweredUserQuestionResultQuery(userId, testId);
        var unAnsweredUserQuestionsResultsQuery = GetUnAnsweredQuestionsResultQuery(userId, testId);
        
        var answeredQuestionsResult = await answeredUserQuestionsResultsQuery.ToListAsync(cancellationToken);
        var unAnsweredQuestionsResult = await unAnsweredUserQuestionsResultsQuery.ToListAsync(cancellationToken);
        
        var userScore = answeredQuestionsResult.Sum(q => q.UserScore);

        var allUserQuestionsResult = answeredQuestionsResult
            .Concat(unAnsweredQuestionsResult)
            .ToList();
        
        return new TestResultResponseDto
        {
            UserId = userId,
            TestId = testId,
            TotalScore = userTest.TotalScore,
            StartingTime = userTest.StartingTime,
            EndingTime = userTest.EndingTime,
            UserTestStatus = userTest.UserTestStatus.ToString(),
            UserScore = userScore,
            QuestionsResults = allUserQuestionsResult
        };
    }

    private IQueryable<QuestionResultResponseDto> GetAnsweredUserQuestionResultQuery(Guid userId, Guid testId)
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
                            QuestionId = a.Question.Id,
                            AnswerId = a.Id,
                            MaxScore = a.Question.MaxScore,
                            QuestionText = a.Question.Text,
                            AnswerText = a.Text,
                            IsAnswerCorrect = a.IsCorrect,
                            UserAnsweredCorrect = a.IsCorrect && ua.AnswerId == a.Id,
                            UserAnsweredWrong = !a.IsCorrect && ua.AnswerId == a.Id
                        }
                    )
            )
            .GroupBy(userAnswers => new { userAnswers.QuestionId, userAnswers.AnswerId })
            .Select(
                group => new
                {
                    QuestionId = group.Key.QuestionId,
                    AnswerId = group.Key.AnswerId,
                    MaxScore = group.First().MaxScore,
                    QuestionText = group.First().QuestionText,
                    AnswerText = group.First().AnswerText,
                    IsAnswerCorrect = group.First().IsAnswerCorrect,
                    UserAnsweredCorrect = group.Any(userAnswers => userAnswers.UserAnsweredCorrect),
                    UserAnsweredWrong = group.Any(userAnswers => userAnswers.UserAnsweredWrong)
                }
            );
    
    
        return userAnswersDetailsQuery
            .GroupBy(userAnswers => userAnswers.QuestionId)
            .Select(
                userQuestion => new
                {
                    QuestionId = userQuestion.Key,
                    QuestionText = userQuestion.First().QuestionText,
                    MaxScore = userQuestion.First().MaxScore,
                    TotalCorrectAnswersCount = userQuestion.Count(userQuestion => userQuestion.IsAnswerCorrect),
                    UserCorrectAnswersCount = userQuestion.Count(userQuestion => userQuestion.UserAnsweredCorrect),
                    UserWrongAnswersCount = userQuestion.Count(userQuestion => userQuestion.UserAnsweredWrong),
                    
                    AnswersResults = userQuestion.Select(
                        userAnswers => new AnswerResultResponseDto
                            {
                                Id = userAnswers.AnswerId,
                                AnswerText = userAnswers.AnswerText,
                                IsCorrect = userAnswers.IsAnswerCorrect,
                                IsUserAnswerSelected = userAnswers.UserAnsweredCorrect || userAnswers.UserAnsweredWrong
                            }
                    ).ToList()
                }
            )
            .Select(
                q => new QuestionResultResponseDto
                {
                    Id = q.QuestionId,
                    QuestionText = q.QuestionText,
                    MaxScore = q.MaxScore,
                    UserScore = (q.UserCorrectAnswersCount - q.UserWrongAnswersCount) <= 0
                        ? 0
                        : (q.UserCorrectAnswersCount - q.UserWrongAnswersCount) *
                          ((q.TotalCorrectAnswersCount != 0) ? (q.MaxScore / q.TotalCorrectAnswersCount) : 0),
                    AnswersResults = q.AnswersResults
                }
            );
    }

    private IQueryable<QuestionResultResponseDto> GetUnAnsweredQuestionsResultQuery(Guid userId, Guid testId)
    {
        return _dataContext.UserQuestions
            .Include(uq => uq.Question)
            .ThenInclude(q => q.QuestionsPool)
            .Include(uq => uq.Question)
            .ThenInclude(q => q.Answers)
            .Where(uq => uq.UserId == userId && 
                         uq.Question.QuestionsPool.TestId == testId &&
                         !_dataContext.UserAnswers
                             .Any(ua => ua.QuestionId == uq.QuestionId 
                                        && ua.UserId == uq.UserId))
            .Select(
                uq => new QuestionResultResponseDto
                {
                    Id = uq.QuestionId,
                    QuestionText = uq.Question.Text,
                    MaxScore = uq.Question.MaxScore,
                    UserScore = 0f,
                    AnswersResults = uq.Question.Answers
                        .Select(
                            a => new AnswerResultResponseDto
                            {
                                AnswerText = a.Text,
                                IsCorrect = a.IsCorrect,
                                IsUserAnswerSelected = false
                            }
                        ).ToList()
                }
            );
    }
    private static Expression<Func<Test, object>> GetSortPropertyForTestToPass(string? sortColumn)
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

    private static Expression<Func<UserTest, object>> GetSortPropertyForStartedTest(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => ut => ut.Test.Name,
            "subject" => ut => ut.Test.Subject,
            "difficulty" => ut => ut.Test.Difficulty,
            "duration" => ut => ut.Test.Duration,
            "startingTime" => ut => ut.StartingTime,
            "endingTime" => ut => ut.EndingTime,
            _ => ut => ut.Test.Name
        };
    }
}