using DapperExtensions;
using MicroServices.Interview.Personnel.API.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.Interview.Personnel.API.Infrastructure.Repositories
{
    /// <summary>
    /// Bu sınıf ayrı bir core katmana alınıp ileride eklenebilecek diğer micro servislerinde kullanması sağlanabilir
    /// </summary>
    public class DapperRepositoryBase<TEntity> : DatabaseConnection, IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        #region Constructor

        public DapperRepositoryBase(IConfiguration configuration) : base(configuration)
        {
        }

        public DapperRepositoryBase(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructor

        #region CRUD operations

        /// <summary>
        /// Silme işlemi
        /// </summary>
        /// <param name="id">Primary id</param>
        public bool Delete(int id)
        {
            var entity = Connection.Get<TEntity>(id);
            return Connection.Delete(entity);
        }

        /// <summary>
        /// Entity ekleme
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        public dynamic Insert(TEntity entity)
        {
            return Connection.Insert(entity);
        }

        /// <summary>
        /// Multiple entity ekleme
        /// </summary>
        /// <param name="entities">Entities listesi</param>
        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await Connection.InsertAsync(entities);
        }

        /// <summary>
        /// Entity güncelle
        /// </summary>
        /// <param name="entity">Entity modeli</param>
        /// <returns>Başarılı ise true değilse false</returns>
        public bool Update(TEntity entity)
        {
            return Connection.Update(entity);
        }

        /// <summary>
        /// tablodaki kayıt sayısı
        /// </summary>
        /// <returns>Kayıt sayısı</returns>
        public int GetCount()
        {
            return Connection.GetList<TEntity>().Count();
        }

        /// <summary>
        /// Id değerine göre tek bir kayıt döndürür
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        public TEntity GetById(int id)
        {
            return Connection.Get<TEntity>(id);
        }

        #endregion CRUD operations
    }
}