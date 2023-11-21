using TaskManager.Core.Models;
using TaskManager.Data.Entities.Base;
using TaskManager.Data.Specifications.Common;

namespace TaskManager.Core.Services.Common
{
    public interface ICommonService<TModel, TEntity>
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        Task<TModel> GetById(int id, bool includeDeleted = false);
        Task<List<TModel>> GetAll(bool includeDeleted = false);
        Task<List<TModel>> GetAll(ICommonSpecification<TEntity> specification, bool includeDeleted = false);
        Task<TModel> GetSingleBySpecification(ICommonSpecification<TEntity> specification, bool includeDeleted = false);
        Task<int> Add(TModel model);
        Task Update(TModel model);
        Task Delete(TModel model);
        Task Delete(int id);
    }
}
