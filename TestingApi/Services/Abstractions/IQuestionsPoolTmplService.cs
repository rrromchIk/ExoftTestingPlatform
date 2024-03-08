using TestingApi.Dto.QuestionsPoolTemplateDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionsPoolTmplService
{
    Task<QuestionsPoolTmplResponseDto?> GetQuestionPoolTmplByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionsPoolTmplExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionsPoolTmplResponseDto> CreateQuestionsPoolTmplAsync(Guid testTemplateId,
        QuestionsPoolTmplDto questionsPoolTmplDto, CancellationToken cancellationToken = default);
    Task UpdateQuestionsPoolTmplAsync(Guid id, QuestionsPoolTmplDto questionsPoolTmplDto,
        CancellationToken cancellationToken = default);
    Task DeleteQuestionsPoolTmplAsync(Guid id, CancellationToken cancellationToken = default);
}