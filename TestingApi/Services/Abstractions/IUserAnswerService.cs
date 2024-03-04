using TestingApi.Dto.UserAnswerDto;

namespace TestingApi.Services.Abstractions;

public interface IUserAnswerService
{
    Task<ICollection<UserAnswerDto>> GetUserAnswersAsync(Guid userId, Guid questionId, CancellationToken cancellationToken = default);
    Task<bool> UserAnswerExistAsync(Guid userId, Guid questionId, Guid answerId, CancellationToken cancellationToken = default);
    Task<UserAnswerDto> CreateUserAnswerAsync(UserAnswerDto userAnswer, CancellationToken cancellationToken = default);
    Task DeleteUserAnswerAsync(Guid userId, Guid questionId, Guid answerId,
        CancellationToken cancellationToken = default);
}