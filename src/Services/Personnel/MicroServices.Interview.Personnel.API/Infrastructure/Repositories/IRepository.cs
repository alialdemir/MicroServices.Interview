using MicroServices.Interview.Personnel.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.Interview.Personnel.API.Infrastructure.Repositories
{
    /// <summary>
    /// Repository arayüzü
    /// </summary>
    /// <typeparam name="T">Generic entity</typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        dynamic Insert(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Delete(int id);

        TEntity GetById(int id);

        int GetCount();
    }
}