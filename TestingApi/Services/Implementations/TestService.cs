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
        var test = _mapper.Map<Test>(entity);
        await _dataContext.AddAsync(entity);
        _logger.LogInformation("{dt}. Create test method. TestDto: {dto}",
            DateTime.Now.ToString(), JsonSerializer.Serialize(testDto));

    public async Task<bool> UpdateTestAsync(Guid id, TestDto testDto)
        _logger.LogInformation(
            "Test to update: {ttu}. Updated test: {ut}",
            JsonSerializer.Serialize(testFounded),
            JsonSerializer.Serialize(updatedTest)
        );
        return await _dataContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteTestAsync(Guid id)
    {
        var entity = await _dataContext.Tests.FindAsync(id);

        _dataContext.Remove(entity);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<ICollection<TestResponseDto>> GetAllTestsAsync()
    {
        var tests =  await _dataContext.Tests.ToListAsync();

       return _mapper.Map<ICollection<TestResponseDto>>(tests);
    }
}