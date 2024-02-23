using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto;
using TestingApi.Dto.Response;
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
    
    public async Task<TestResponseDto> GetTestByIdAsync(Guid id)
    {
        var tests = await _dataContext.Tests.AsNoTracking().FirstAsync(e => e.Id == id);

        return _mapper.Map<TestResponseDto>(tests);
    }

    public async Task<bool> TestExistsAsync(Guid id)
    {
        return await _dataContext.Tests.AnyAsync(e => e.Id.Equals(id));
    }
    
    public async Task<TestResponseDto> CreateTestAsync(TestDto testDto)
    {
        _logger.LogInformation(
            "{dt}. Create test method. TestDto: {dto}",
            DateTime.Now.ToString(),
            JsonSerializer.Serialize(testDto)
        );
        var testToAdd = _mapper.Map<Test>(testDto);
        var createdTest = await _dataContext.AddAsync(testToAdd);

        await _dataContext.SaveChangesAsync();
        
        return _mapper.Map<TestResponseDto>(createdTest.Entity);
    }

    public async Task<bool> UpdateTestAsync(Guid id, TestDto testDto)
    {
        var testFounded = await _dataContext.FindAsync<Test>(id);
        var updatedTest = _mapper.Map<Test>(testDto);

        _logger.LogInformation(
            "Test to update: {ttu}. Updated test: {ut}",
            JsonSerializer.Serialize(testFounded),
            JsonSerializer.Serialize(updatedTest)
        );
        
        testFounded.Name = updatedTest.Name;
        testFounded.Subject = updatedTest.Subject;
        testFounded.Difficulty = updatedTest.Difficulty;
        testFounded.Duration = updatedTest.Duration;
        
        return await _dataContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteTestAsync(Guid id)
    {
        var entity = await _dataContext.Tests.FindAsync(id);

        _dataContext.Remove(entity);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<PagedList<TestResponseDto>> GetAllTestsAsync(TestFiltersDto testFiltersDto)
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
            testFiltersDto.PageSize
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