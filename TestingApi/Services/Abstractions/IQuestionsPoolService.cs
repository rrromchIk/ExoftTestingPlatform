using TestingApi.Dto.QuestionsPoolDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionsPoolService
{
    Task<QuestionsPoolResponseDto?> GetQuestionPoolByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionsPoolExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionsPoolResponseDto> CreateQuestionsPoolAsync(Guid testId, QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default);
    Task UpdateQuestionsPoolAsync(Guid id, QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default);
    
    Task DeleteQuestionsPoolAsync(Guid id, CancellationToken cancellationToken = default);
}