using TestingApi.Dto.QuestionTemplateDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionTmplService
{
    Task<QuestionTmplResponseDto?> GetQuestionTmplByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionTmplExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionTmplResponseDto> CreateQuestionTmplAsync(Guid questionsPoolTemplateId,
        QuestionTmplWithAnswerTmplDto questionTmplWithAnswerTmplDto, CancellationToken cancellationToken = default);
    Task UpdateQuestionTmplAsync(Guid id, QuestionTmplDto questionDto, CancellationToken cancellationToken = default);
    Task DeleteQuestionTmplAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ICollection<QuestionTmplResponseDto>> GetQuestionTmplsByQuestionsPoolTmplIdAsync(Guid questionsPoolTmplId,
        CancellationToken cancellationToken = default);
}