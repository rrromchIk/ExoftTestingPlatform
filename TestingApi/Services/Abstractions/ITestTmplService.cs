using TestingApi.Dto.TestTemplateDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface ITestTmplService
{ 
    Task<TestTmplResponseDto?> GetTestTmplByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TestTmplWithQpTmplsResponseDto?> GetTestTmplWithQuestionsPoolsTmplByIdAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> TestTmplExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TestTmplWithQpTmplsResponseDto> CreateTestTmplAsync(
        TestTmplWithQuestionsPoolTmplDto testWithQuestionsPoolsDto,
        CancellationToken cancellationToken = default
    );

    Task UpdateTestTmplAsync(Guid id, TestTmplDto testTmplDto, CancellationToken cancellationToken = default);
    
    Task DeleteTestTmplAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedList<TestTmplResponseDto>> GetAllTestsTmplsAsync(TestTemplateFiltersDto filtersDto,
        CancellationToken cancellationToken = default);
    
    Task<ICollection<TestTmplShortInfoResponseDto>> GetAllTestsTmplsShortInfoAsync(CancellationToken cancellationToken = default);

    public Task<PagedList<TestTmplResponseDto>> GetTestsTmplsByAuthorIdAsync(
        Guid authorId, TestTemplateFiltersDto filtersDto, CancellationToken cancellationToken = default
    );
}