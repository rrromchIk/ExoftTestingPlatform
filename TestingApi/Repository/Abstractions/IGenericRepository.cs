using TestingApi.Models;

namespace TestingApi.Repository.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetById(Guid id);
    Task<bool> Exists(Guid id);
    Task<bool> Create(TEntity entity);
    Task<bool> Update(Guid id, TEntity entity);
    Task<bool> Delete(Guid id);
}