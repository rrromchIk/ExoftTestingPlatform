using TestingApi.Dto;
using TestingApi.Dto.TestDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface ITestService
{
    Task<TestResponseDto?> GetTestByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TestWithQuestionsPoolResponseDto?> GetTestWithQuestionsPoolsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> TestExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TestWithQuestionsPoolResponseDto> CreateTestAsync(TestWithQuestionsPoolsDto testWithQuestionsPoolsDto, CancellationToken cancellationToken = default);
    Task UpdateTestAsync(Guid id, TestDto testDto, CancellationToken cancellationToken = default);
    Task DeleteTestAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<TestResponseDto>> GetAllTestsAsync(FiltersDto filtersDto,
        CancellationToken cancellationToken = default);

    Task UpdateIsPublishedAsync(Guid id, bool isPublished, CancellationToken cancellationToken = default);
}