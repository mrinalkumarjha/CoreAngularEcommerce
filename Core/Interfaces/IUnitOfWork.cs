using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    // IDisposable will look for Dispose method inside this class
    // When transaction will be finished it will dispose our context.
    public interface IUnitOfWork: IDisposable
    {
         IGenericRepository<TEntity> Repository<TEntity>() where TEntity: BaseEntity;

        // This will return number of changes to database.
         Task<int> Complete();
    }
}