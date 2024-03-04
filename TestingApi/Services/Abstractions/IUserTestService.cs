using TestingApi.Dto.TestDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface IUserTestService
{
    Task<UserTestResponseDto?> GetUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);

    Task<PagedList<TestToPassResponseDto>> GetAllTestsForUserAsync(TestFiltersDto testFiltersDto, Guid userId,
        CancellationToken cancellationToken = default);
    Task<PagedList<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(TestFiltersDto testFiltersDto,
        Guid userId, CancellationToken cancellationToken = default);

    Task<ICollection<TestPassingQuestionsPoolResponseDto>> GetQuestionsForUserTest(Guid userId, Guid testId,
        CancellationToken cancellationToken = default);
    Task<bool> UserTestExistsAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task<UserTestResponseDto> CreateUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task CompleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task DeleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
}