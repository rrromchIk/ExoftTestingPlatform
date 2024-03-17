using TestingApi.Dto.UserQuestionDto;

namespace TestingApi.Services.Abstractions;

public interface IUserQuestionService
{
    Task<ICollection<QuestionsPoolDetailsDto>> GetAllQuestionsForTestAsync(Guid testId,
        CancellationToken cancellationToken = default);
    Task<ICollection<UserQuestionDetailsResponseDto>> GetUserQuestionsDetails(Guid userId, Guid testId,
        CancellationToken cancellationToken = default);
    Task CreateUserQuestions(ICollection<UserQuestionDto> userQuestionsDto, CancellationToken cancellationToken = default);
}