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
    
    public async Task<TestResponseDto> GetByIdAsync(Guid id)
    {
        var tests = await _dataContext.Tests.AsNoTracking().FirstAsync(e => e.Id == id);

        return _mapper.Map<TestResponseDto>(tests);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dataContext.Tests.AnyAsync(e => e.Id.Equals(id));
    }
    
    public async Task<bool> CreateAsync(TestDto entity)
    {
        var test = _mapper.Map<Test>(entity);
        await _dataContext.AddAsync(entity);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Guid id, TestDto entity)
    {

        var entityFounded = await _dataContext.FindAsync<Test>(id);
        var updatedTest = _mapper.Map<Test>(entity);

        //....
        
        return await _dataContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
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