﻿using TestingApi.Dto;
using TestingApi.Dto.TestResultDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Helpers;

namespace TestingApi.Services.Abstractions;

public interface IUserTestService
{
    Task<UserTestResponseDto?> GetUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);

    Task<PagedList<TestToPassResponseDto>> GetAllTestsForUserAsync(UserTestFilters filtersDto, Guid userId,
        CancellationToken cancellationToken = default);
    Task<PagedList<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(UserTestFilters filtersDto,
        Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserTestExistsAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task<UserTestResponseDto> CreateUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
    Task CompleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);

    Task<TestResultResponseDto> GetUserTestResults(Guid userId, Guid testId,
        CancellationToken cancellationToken = default);
    Task DeleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default);
}