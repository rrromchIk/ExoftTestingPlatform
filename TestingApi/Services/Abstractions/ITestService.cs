using TestingApi.Dto;
using TestingApi.Dto.Response;

namespace TestingApi.Services.Abstractions;

public interface ITestService
{
    Task<TestResponseDto> GetTestByIdAsync(Guid id);
    Task<bool> TestExistsAsync(Guid id);
    Task<TestResponseDto> CreateTestAsync(TestDto testDto);
    Task<bool> UpdateTestAsync(Guid id, TestDto testDto);
    Task<bool> DeleteTestAsync(Guid id);
    Task<ICollection<TestResponseDto>> GetAllTestsAsync();
}