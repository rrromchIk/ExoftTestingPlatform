using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Models;
using TestingApi.Repo.Abstractions;
using TestingApi.Repository.Abstractions;

namespace TestingApi.Repository.Implementations;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DataContext _dataContext;
    protected DbSet<TEntity> _dbSet;

    public GenericRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = _dataContext.Set<TEntity>();
    }

    public async Task<TEntity> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id.Equals(id));
    }
    
    public async Task<bool> Create(TEntity entity)
    {
        await _dataContext.AddAsync(entity);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(Guid id, TEntity entity)
    {
        var entityFounded = await _dataContext.FindAsync<TEntity>(id);

        //....
        
        return await _dataContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        _dataContext.Remove(entity);
        return await _dataContext.SaveChangesAsync() > 0;
    }
}