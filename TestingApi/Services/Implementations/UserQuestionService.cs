using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.UserQuestionDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserQuestionService : IUserQuestionService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserQuestionService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
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

    public async Task<ICollection<UserQuestionDetailsResponseDto>> GetUserQuestionsDetails(Guid userId, Guid testId, 
        CancellationToken cancellationToken)
    {
        return await _dataContext.UserQuestions
            .Include(uq => uq.Question)
            .ThenInclude(q => q.QuestionsPool)
            .Where(uq => uq.UserId == userId && uq.Question.QuestionsPool.TestId == testId)
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
            ).ToListAsync(cancellationToken);
    }

    public async Task CreateUserQuestions(ICollection<UserQuestionDto> userQuestionsDto, CancellationToken cancellationToken = default)
    {
        var userQuestionsToAdd = _mapper.Map<ICollection<UserQuestion>>(userQuestionsDto);

        await _dataContext.AddRangeAsync(userQuestionsToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}