using TestingApi.Models;
using TestingApi.Repo.Abstractions;

namespace TestingApi.Repository.Abstractions;

public interface ITestRepository : IGenericRepository<Test>
{
    Task<ICollection<Test>> GetAllTests();
}