using TestingApi.Models;

namespace TestingApi.Services.Abstractions;

public interface IQuestionService 
{
    Task<Question> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(Test entity);
    Task<bool> UpdateAsync(Guid id, Question entity);
    Task<bool> DeleteAsync(Guid id);
    Task<ICollection<Question>> GetQuestionsByTestIdAsync(Guid testId);
}