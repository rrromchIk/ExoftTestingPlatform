using TestingApi.Dto.QuestionsPoolDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionsPoolService
{
    Task<PagedList<QuestionsPoolResponseDto>> GetQuestionPoolsByTestIdAsync(Guid testId,
        CancellationToken cancellationToken = default);

    Task<QuestionsPoolResponseDto?> GetQuestionPoolByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionsPoolExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionsPoolResponseDto> CreateQuestionsPoolAsync(QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default);
    
    Task<bool> UpdateQuestionsPoolAsync(Guid id, QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default);
    
    Task<bool> DeleteQuestionsPoolAsync(Guid id, CancellationToken cancellationToken = default);
}