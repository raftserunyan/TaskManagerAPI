﻿using System.Collections;
using TaskManager.Data.DAO;
using TaskManager.Data.Entities.Base;
using TaskManager.Data.Repositories;

namespace TaskManager.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagerDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(TaskManagerDbContext context)
        {
            _context = context;
        }

        public ICommonRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(CommonRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (ICommonRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

            // TODO
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    throw new EntityUpdateConcurrencyException(ex.Entries);
            //}
            //catch (DbUpdateException ex) when (ex.ForeignKeyConstraintConflictOnInsert())
            //{
            //    throw new BadDataException("Related entity not found.");
            //}
        }
    }
}
