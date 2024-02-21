using TestingApi.Models;

namespace TestingApi.Services.Abstractions;

public interface ITestService
{
    Task<Test> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(Test entity);
    Task<bool> UpdateAsync(Guid id, Test entity);
    Task<bool> DeleteAsync(Guid id);
    Task<ICollection<Test>> GetAllTestsAsync();
}