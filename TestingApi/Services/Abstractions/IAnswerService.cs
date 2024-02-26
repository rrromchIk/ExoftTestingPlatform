using TestingApi.Dto.Request;
using TestingApi.Dto.Response;

namespace TestingApi.Services.Abstractions;

public interface IAnswerService
{
    Task<ICollection<AnswerResponseDto>> GetAnswersByQuestionIdAsync(Guid questionId,
        CancellationToken cancellationToken = default);
    Task<bool> UpdateAnswerAsync(Guid id, AnswerDto answerDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default);
    
}