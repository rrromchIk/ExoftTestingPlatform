using TestingApi.Dto.AnswerDto;

namespace TestingApi.Services.Abstractions;

public interface IAnswerService
{
    Task<AnswerResponseDto?> GetAnswerById(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnswerExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AnswerResponseDto> CreateAnswerAsync(Guid questionId, AnswerDto answerDto, CancellationToken cancellationToken = default);
    Task UpdateAnswerAsync(Guid id, AnswerDto answerDto, CancellationToken cancellationToken = default);
    Task DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default);
    
}