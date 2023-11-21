using TaskManager.Data.Entities.Base;
using TaskManager.Data.Repositories;

namespace TaskManager.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICommonRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task SaveChangesAsync();
    }
}
