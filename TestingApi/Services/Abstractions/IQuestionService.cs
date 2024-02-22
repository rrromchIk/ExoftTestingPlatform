using TestingApi.Models;

namespace TestingApi.Services.Abstractions;

public interface IQuestionService 
{
    Task<Question> GetQuestionByIdAsync(Guid id);
    Task<bool> QuestionExistsAsync(Guid id);
    Task<bool> CreateQuestionAsync(Test entity);
    Task<bool> UpdateQuestionAsync(Guid id, Question entity);
    Task<bool> DeleteQuestionAsync(Guid id);
    Task<ICollection<Question>> GetQuestionsByTestIdAsync(Guid testId);
}