using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Models;
using TestingApi.Repository.Abstractions;

namespace TestingApi.Repository.Implementations;

public class TestRepository : GenericRepository<Test>, ITestRepository
{
    public TestRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<ICollection<Test>> GetAllTests()
    {
        return await _dbSet.ToListAsync();
    }
}