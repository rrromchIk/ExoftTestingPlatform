using TestingApi.Dto.QuestionsPoolTemplateDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionsPoolTemplateService
{
    Task<QuestionsPoolTemplateResponseDto?> GetQuestionPoolTemplateByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionsPoolTemplateExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionsPoolTemplateResponseDto> CreateQuestionsPoolTemplateAsync(Guid testTemplateId,
        QuestionsPoolTemplateDto questionsPoolTemplateDto, CancellationToken cancellationToken = default);
    Task UpdateQuestionsPoolTemplateAsync(Guid id, QuestionsPoolTemplateDto questionsPoolTemplateDto,
        CancellationToken cancellationToken = default);
    Task DeleteQuestionsPoolTemplateAsync(Guid id, CancellationToken cancellationToken = default);
}