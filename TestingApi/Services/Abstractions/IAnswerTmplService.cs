using TestingApi.Dto.AnswerTemplateDto;

namespace TestingApi.Services.Abstractions;

public interface IAnswerTmplService
{
    Task<AnswerTmplResponseDto?> GetAnswerTmplById(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnswerTmplExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AnswerTmplResponseDto> CreateAnswerTmplAsync(Guid questionTemplateId,
        AnswerTmplDto answerTmplDto, CancellationToken cancellationToken = default);
    Task UpdateAnswerTmplAsync(Guid id, AnswerTmplDto answerTmplDto, CancellationToken cancellationToken = default);
    Task DeleteAnswerTmplAsync(Guid id, CancellationToken cancellationToken = default);
}