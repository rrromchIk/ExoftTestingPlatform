using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Data;
using TestingApi.Dto.TestDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models.Test;
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

    public async Task UpdateTestAsync(Guid id, TestUpdateDto testUpdateDto, CancellationToken cancellationToken = default)
    {
        var testFounded = await _dataContext.Tests.FirstAsync(e => e.Id == id, cancellationToken);
        var updatedTest = _mapper.Map<Test>(testUpdateDto);
        
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

    public async Task<PagedList<TestResponseDto>> GetAllTestsAsync(TestFiltersDto filtersDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Test> testsQuery = _dataContext.Tests;

        testsQuery = ApplyFilters(testsQuery, filtersDto);
        
        testsQuery = filtersDto.SortOrder?.ToLower() == "asc"
            ? testsQuery.OrderBy(GetSortProperty(filtersDto.SortColumn, "asc"))
            : testsQuery.OrderByDescending(GetSortProperty(filtersDto.SortColumn, "desc"));

        var tests = await PagedList<Test>.CreateAsync(
            testsQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<TestResponseDto>>(tests);
    }

    public async Task UpdateIsPublishedAsync(Guid id, bool isPublished, CancellationToken cancellationToken = default)
    {
        var testToPublish = await _dataContext.Tests.FirstAsync(t => t.Id == id, cancellationToken);

        testToPublish.IsPublished = isPublished;
        
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
    
    
    private static IQueryable<Test> ApplyFilters(IQueryable<Test> query, TestFiltersDto testFiltersDto) {
        if (testFiltersDto.Published != null) {
            query = query
                .Where(t => t.IsPublished == testFiltersDto.Published);
        }
        
        if (!string.IsNullOrEmpty(testFiltersDto.Difficulty)) {
            if (Enum.TryParse(typeof(TestDifficulty), testFiltersDto.Difficulty, true, out var difficultyValue)) {
                query = query.Where(t => t.Difficulty == (TestDifficulty)difficultyValue);
            }
        }
        
        if (!string.IsNullOrEmpty(testFiltersDto.TemplateId)) {
            query = query.Where(t => t.TemplateId.ToString() == testFiltersDto.TemplateId);
        }
        
        if (!string.IsNullOrWhiteSpace(testFiltersDto.SearchTerm))
        {
            query = query.Where(
                t =>
                    t.Name.Contains(testFiltersDto.SearchTerm) ||
                    t.Subject.Contains(testFiltersDto.SearchTerm)
            );
        }

        return query;
    }

    private static Expression<Func<Test, object>> GetSortProperty(string? sortColumn, string sortOrder)
    {
        return sortColumn?.ToLower() switch
        {
            "duration" => t => t.Duration,
            "modificationdate" => t => t.ModifiedTimestamp == null 
                ? sortOrder == "asc" 
                    ? DateTime.MaxValue
                    : DateTime.MinValue
                : t.ModifiedTimestamp,
            "creationdate" => t => t.CreatedTimestamp,
            _ => t => t.CreatedTimestamp
        };
    }
}