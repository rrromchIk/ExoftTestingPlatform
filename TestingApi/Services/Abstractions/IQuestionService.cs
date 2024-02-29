using TestingApi.Dto.QuestionDto;

namespace TestingApi.Services.Abstractions;

public interface IQuestionService 
{
    Task<QuestionResponseDto?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> QuestionExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionResponseDto> CreateQuestionAsync(Guid questionsPoolId, QuestionWithAnswersDto questionWithAnswersDto,
        CancellationToken cancellationToken = default);
    Task UpdateQuestionAsync(Guid id, QuestionDto questionDto, CancellationToken cancellationToken = default);
    Task DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ICollection<QuestionResponseDto>> GetQuestionsByQuestionsPoolIdAsync(Guid questionsPoolId,
        CancellationToken cancellationToken = default);
}