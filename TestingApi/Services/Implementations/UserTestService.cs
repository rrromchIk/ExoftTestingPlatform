using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserTestService : IUserTestService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserTestService> _logger;

    public UserTestService(DataContext dataContext, ILogger<UserTestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserTestResponseDto?> GetUserTestAsync(Guid userId, Guid testId,
        CancellationToken cancellationToken = default)
    {
        var userTestFounded = await _dataContext.UserTests
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        return _mapper.Map<UserTestResponseDto>(userTestFounded);
    }

    public async Task<ICollection<TestToPassResponseDto>> GetAllTestsForUserAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dataContext.Tests
            .Include(t => t.UserTests)
            .Select(
                t => new TestToPassResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Subject = t.Subject,
                    Duration = t.Duration,
                    Difficulty = t.Difficulty.ToString(),
                    UserTestStatus = t.UserTests
                        .Where(ut => ut.UserId == userId)
                        .Select(ut => ut.UserTestStatus)
                        .FirstOrDefault()
                        .ToString()
                }
            ).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<StartedTestResponseDto>> GetAllStartedTestsForUserAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserTests
            .Include(ut => ut.Test)
            .Select(ut => new StartedTestResponseDto
            {
                Result = ut.Result,
                StartingTime = ut.StartingTime,
                EndingTime = ut.EndingTime,
                UserTestStatus = ut.UserTestStatus.ToString(),
                Test = _mapper.Map<TestResponseDto>(ut.Test)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UserTestExistsAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserTests
            .AnyAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);
    }

    public async Task<UserTestResponseDto> CreateUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        var userTestToAdd = new UserTest()
        {
            UserId = userId,
            TestId = testId,
            StartingTime = DateTime.Now,
            UserTestStatus = UserTestStatus.InProcess
        };

        var addedUserTest = await _dataContext.AddAsync(userTestToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserTestResponseDto>(addedUserTest.Entity);
    }

    public async Task CompleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        var userTestToComplete = await _dataContext.UserTests
            .FirstAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        userTestToComplete.UserTestStatus = UserTestStatus.Completed;
        userTestToComplete.Result = 0f;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserTestAsync(Guid userId, Guid testId, CancellationToken cancellationToken = default)
    {
        var userTestToDelete = await _dataContext.UserTests
            .FirstAsync(ut => ut.UserId == userId && ut.TestId == testId, cancellationToken);

        _dataContext.Remove(userTestToDelete);

        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}