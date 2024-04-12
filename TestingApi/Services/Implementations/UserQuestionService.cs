using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.UserQuestionDto;
using TestingAPI.Exceptions;
using TestingApi.Models;
using TestingApi.Models.Test;
using TestingApi.Services.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestingApi.Services.Implementations;

public class UserQuestionService : IUserQuestionService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IUserTestService _userTestService;
    private readonly ILogger<UserQuestionService> _logger;

    public UserQuestionService(DataContext dataContext, IMapper mapper, IUserTestService userTestService,
        ILogger<UserQuestionService> logger)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _userTestService = userTestService;
        _logger = logger;
    }

    public async Task<ICollection<QuestionsPoolDetailsDto>> GetAllQuestionsForTestAsync(
        Guid testId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPools
            .Include(qp => qp.Questions)
            .Where(qp => qp.TestId == testId)
            .Select(
                qp => new QuestionsPoolDetailsDto
                {
                    QuestionsPoolId = qp.Id,
                    GenerationStrategy = qp.GenerationStrategy.ToString(),
                    NumOfQuestionsToBeGenerated = qp.NumOfQuestionsToBeGenerated,
                    QuestionsId = qp.Questions.Select(q => q.Id).ToList()
                }
            ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserQuestionDetailsResponseDto>> GetUserQuestions(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var userTest = await _userTestService.GetUserTestAsync(userId, testId, cancellationToken);

        if (userTest != null)
        {
            return await GetUserQuestionsForExistingTest(userId, testId, cancellationToken);
        }

        ICollection<UserQuestionDto> userQuestions = Enumerable.Empty<UserQuestionDto>().ToList();

        try
        {
            await _dataContext.Database.BeginTransactionAsync(cancellationToken);
            await _userTestService.CreateUserTestAsync(userId, testId, cancellationToken);

            userQuestions = await GetUserQuestionsForNewTest(userId, testId, cancellationToken);

            await CreateUserQuestions(userQuestions, cancellationToken);
            
            await _dataContext.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _dataContext.Database.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(e.Message);
            throw new ApiException(e.Message, StatusCodes.Status500InternalServerError);
        }
        
        
        return userQuestions.Select(
            uq => new UserQuestionDetailsResponseDto
            {
                UserId = uq.UserId.GetValueOrDefault(),
                QuestionId = uq.QuestionId.GetValueOrDefault(),
                IsAnswered = false
            }
        ).ToList();
    }

    public async Task CreateUserQuestions(ICollection<UserQuestionDto> userQuestionsDto,
        CancellationToken cancellationToken = default)
    {
        var userQuestionsToAdd = _mapper.Map<ICollection<UserQuestion>>(userQuestionsDto);
        foreach (var userQuestion in userQuestionsToAdd)
        {
            userQuestion.CreatedTimestamp = DateTime.Now;
        }
        
        await _dataContext.AddRangeAsync(userQuestionsToAdd, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<ICollection<UserQuestionDto>> GetUserQuestionsForNewTest(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("get user questions for new test");
        var allQuestionsInTheTest = await GetAllQuestionsForTestAsync(testId, cancellationToken);
        return GenerateUserQuestions(userId, allQuestionsInTheTest);
    }

    private ICollection<UserQuestionDto> GenerateUserQuestions(Guid userId,
        ICollection<QuestionsPoolDetailsDto> questionsPools)
    {
        var concatenatedQuestions = new List<UserQuestionDto>();

        foreach (var pool in questionsPools)
        {
            _logger.LogInformation("question pool: {q}", JsonSerializer.Serialize(pool));
            if (pool.GenerationStrategy == GenerationStrategy.Randomly.ToString())
            {
                _logger.LogInformation("random strategy: {q}", JsonSerializer.Serialize(pool.QuestionsId));
                pool.QuestionsId = ShuffleArray(pool.QuestionsId.ToList());
                _logger.LogInformation("random strategy after shuffling: {q}", JsonSerializer.Serialize(pool.QuestionsId));
            }

            var numToConcatenate = Math.Min(pool.NumOfQuestionsToBeGenerated, pool.QuestionsId.Count);
            concatenatedQuestions.AddRange(
                pool.QuestionsId
                    .Take(numToConcatenate)
                    .Select(
                        questionId => new UserQuestionDto
                        {
                            UserId = userId,
                            QuestionId = questionId
                        }
                    )
            );
        }

        return concatenatedQuestions;
    }

    private static ICollection<T> ShuffleArray<T>(IList<T> array)
    {
        var rng = new Random();
        var n = array.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (array[k], array[n]) = (array[n], array[k]);
        }

        return array;
    }

    private async Task<ICollection<UserQuestionDetailsResponseDto>> GetUserQuestionsForExistingTest(Guid userId,
        Guid testId,
        CancellationToken cancellationToken)
    {
        return await _dataContext.UserQuestions
            .Include(uq => uq.Question)
            .ThenInclude(q => q.QuestionsPool)
            .Where(uq => uq.UserId == userId && uq.Question.QuestionsPool.TestId == testId)
            .OrderBy(uq => uq.CreatedTimestamp)
            .Select(
                uq => new UserQuestionDetailsResponseDto
                {
                    UserId = uq.UserId,
                    QuestionId = uq.QuestionId,
                    IsAnswered = _dataContext.UserAnswers
                        .Any(
                            ua => ua.UserId == uq.UserId &&
                                  ua.QuestionId == uq.QuestionId
                        )
                }
            )
            .ToListAsync(cancellationToken);
    }
}