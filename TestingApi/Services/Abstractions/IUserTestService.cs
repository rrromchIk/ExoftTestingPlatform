using TestingApi.Dto.UserTestDto;
using TestingApi.Models;

namespace TestingApi.Services.Abstractions;

public interface IUserTestService
{
    Task<UserTestResponseDto?> GetUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);

    Task<ICollection<TestToPassResponseDto>> GetAllTestsForUserAsync(Guid userId,
        CancellationToken cancellationToken = default);

    Task<ICollection<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(Guid userId,
        CancellationToken cancellationToken = default);
    Task<bool> UserTestExistsAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task<UserTestResponseDto> CreateUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task CompleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task DeleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
}