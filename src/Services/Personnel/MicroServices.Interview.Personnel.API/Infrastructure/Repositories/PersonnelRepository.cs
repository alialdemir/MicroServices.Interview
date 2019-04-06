using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MicroServices.Interview.Personnel.API.Infrastructure.Repositories
{
    public class PersonnelRepository : DapperRepositoryBase<Tables.Personnel>, IPersonnelRepository
    {
        #region Constructor

        public PersonnelRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Tüm personel listesini döndürür
        /// </summary>
        /// <returns>Personel listesi</returns>
        public IEnumerable<Model.Personnel> GetAllPersonnels()
        {
            string sql = "Select * from Personnel";

            return Connection.Query<Model.Personnel>(sql).ToList();
        }

        #endregion Methods
    }
}