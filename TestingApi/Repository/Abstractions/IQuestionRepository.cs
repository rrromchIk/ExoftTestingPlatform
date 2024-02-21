using TestingApi.Models;
using TestingApi.Repository.Abstractions;

namespace TestingApi.Repo.Abstractions;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<ICollection<Question>> GetQuestionsByTestId(Guid testId);
}