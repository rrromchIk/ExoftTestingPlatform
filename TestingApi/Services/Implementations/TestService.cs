using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class TestService : ITestService
{
    private readonly DataContext _dataContext;
    public TestService(DataContext dataContext)
    {
    }
    public async Task<Test> GetByIdAsync(Guid id)
    {
        return await _dataContext.Tests.AsNoTracking().FirstAsync(e => e.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dataContext.Tests.AnyAsync(e => e.Id.Equals(id));
    }
    
    public async Task<bool> CreateAsync(Test entity)
    {
        await _dataContext.AddAsync(entity);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Guid id, Test entity)
    {
        var entityFounded = await _dataContext.FindAsync<Test>(id);

        //....
        
        return await _dataContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dataContext.Tests.FindAsync(id);

        _dataContext.Remove(entity);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<ICollection<Test>> GetAllTestsAsync()
    {
        return await _dataContext.Tests.ToListAsync();
    }
}