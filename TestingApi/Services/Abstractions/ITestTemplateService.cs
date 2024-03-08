using TestingApi.Dto;
using TestingApi.Dto.TestTemplateDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface ITestTemplateService
{ 
    Task<TestTemplateResponseDto?> GetTestTemplateByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TestTemplateWithQpTemplatesResponseDto?> GetTestTemplateWithQuestionsPoolsTemplatesByIdAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> TestTemplateExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TestTemplateWithQpTemplatesResponseDto> CreateTestTemplateAsync(
        TestTemplateWithQpTemplateDto testWithQuestionsPoolsDto,
        CancellationToken cancellationToken = default
    );

    Task UpdateTestTemplateAsync(Guid id, TestTemplateDto testTemplateDto, CancellationToken cancellationToken = default);
    
    Task DeleteTestTemplateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedList<TestTemplateResponseDto>> GetAllTestsTemplatesAsync(FiltersDto filtersDto,
        CancellationToken cancellationToken = default);
}