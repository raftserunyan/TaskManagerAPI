using TaskManager.Data.Entities.Base;
using TaskManager.Data.Specifications.Common;
using TaskManager.Shared.Enums;

namespace TaskManager.Data.Repositories
{
    public interface ICommonRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAll(bool includeDeleted = false);

        Task<TEntity> GetById(int id, bool includeDeleted = false);

        Task<List<TEntity>> GetAllBySpecification(ICommonSpecification<TEntity> commonSpecification, bool includeDeleted = false);

        Task<TEntity> GetSingleBySpecification(ICommonSpecification<TEntity> commonSpecification, bool includeDeleted = false);

        Task Add(TEntity entity);

        Task AddRange(List<TEntity> entities);

        Task Delete(int id, DeleteOptions deleteOption = DeleteOptions.Soft);

        void Delete(TEntity entity, DeleteOptions deleteOption = DeleteOptions.Soft);

        void DeleteRange(IEnumerable<TEntity> entities, DeleteOptions deleteOption = DeleteOptions.Soft);
    }
}
