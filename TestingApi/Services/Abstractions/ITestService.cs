using TestingApi.Dto;
using TestingApi.Dto.Response;

namespace TestingApi.Services.Abstractions;

public interface ITestService
{
    Task<TestResponseDto> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(TestDto entity);
    Task<bool> UpdateAsync(Guid id, TestDto entity);
    Task<bool> DeleteAsync(Guid id);
    Task<ICollection<TestResponseDto>> GetAllTestsAsync();
}