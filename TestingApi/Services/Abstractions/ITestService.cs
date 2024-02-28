using TestingApi.Dto.TestDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface ITestService
{
    Task<TestResponseDto?> GetTestByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> TestExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TestResponseDto> CreateTestAsync(TestDto testDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateTestAsync(Guid id, TestDto testDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTestAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<TestResponseDto>> GetAllTestsAsync(TestFiltersDto testFiltersDto,
        CancellationToken cancellationToken = default);
}