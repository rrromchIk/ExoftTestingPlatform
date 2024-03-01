using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Data;
using TestingApi.Dto.TestDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class TestService : ITestService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestService> _logger;

    public TestService(DataContext dataContext, ILogger<TestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TestResponseDto?> GetTestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var test = await _dataContext.Tests
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return _mapper.Map<TestResponseDto>(test);
    }

    public async Task<TestWithQuestionsPoolResponseDto?> GetTestWithQuestionsPoolsByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var test = await _dataContext.Tests
            .AsNoTracking()
            .Include(t => t.QuestionsPools)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return _mapper.Map<TestWithQuestionsPoolResponseDto>(test);
    }

    public async Task<bool> TestExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Tests
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<TestWithQuestionsPoolResponseDto> CreateTestAsync(
        TestWithQuestionsPoolsDto testWithQuestionsPoolsDto, CancellationToken cancellationToken = default)
    {
        var testToAdd = _mapper.Map<Test>(testWithQuestionsPoolsDto);

        //TODO check qp unique
        if (!testWithQuestionsPoolsDto.QuestionsPools.IsNullOrEmpty())
        {
            testToAdd.QuestionsPools =
                _mapper.Map<ICollection<QuestionsPool>>(testWithQuestionsPoolsDto.QuestionsPools);
        }

        var collision = await _dataContext.Tests
            .AnyAsync(
                t => t.Name == testToAdd.Name,
                cancellationToken
            );

        if (collision)
            throw new ApiException("Test name has to be unique", StatusCodes.Status409Conflict);

        var createdTest = await _dataContext.AddAsync(testToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TestWithQuestionsPoolResponseDto>(createdTest.Entity);
    }

    public async Task UpdateTestAsync(Guid id, TestDto testDto, CancellationToken cancellationToken = default)
    {
        var testFounded = await _dataContext.Tests.FirstAsync(e => e.Id == id, cancellationToken);
        var updatedTest = _mapper.Map<Test>(testDto);
        
        var collision = await _dataContext.Tests
            .AnyAsync(
                t => 
                    t.Name == updatedTest.Name &&
                     t.Id != testFounded.Id,
                cancellationToken
            );

        if (collision)
            throw new ApiException("Test name has to be unique", StatusCodes.Status409Conflict);

        testFounded.Name = updatedTest.Name;
        testFounded.Subject = updatedTest.Subject;
        testFounded.Difficulty = updatedTest.Difficulty;
        testFounded.Duration = updatedTest.Duration;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var testToDelete = await _dataContext.Tests
            .FirstAsync(e => e.Id == id, cancellationToken);

        _dataContext.Remove(testToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedList<TestResponseDto>> GetAllTestsAsync(TestFiltersDto testFiltersDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Test> testsQuery = _dataContext.Tests;

        if (!string.IsNullOrWhiteSpace(testFiltersDto.SearchTerm))
        {
            testsQuery = testsQuery.Where(
                t =>
                    t.Name.Contains(testFiltersDto.SearchTerm) ||
                    t.Subject.Contains(testFiltersDto.SearchTerm)
            );
        }

        testsQuery = testFiltersDto.SortOrder?.ToLower() == "desc"
            ? testsQuery.OrderByDescending(GetSortProperty(testFiltersDto.SortColumn))
            : testsQuery.OrderBy(GetSortProperty(testFiltersDto.SortColumn));

        var tests = await PagedList<Test>.CreateAsync(
            testsQuery,
            testFiltersDto.Page,
            testFiltersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<TestResponseDto>>(tests);
    }

    private static Expression<Func<Test, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => t => t.Name,
            "subject" => t => t.Subject,
            "difficulty" => t => t.Difficulty,
            "duration" => t => t.Duration,
            "creationTime" => t => t.CreatedTimestamp,
            _ => t => t.Id
        };
    }
}