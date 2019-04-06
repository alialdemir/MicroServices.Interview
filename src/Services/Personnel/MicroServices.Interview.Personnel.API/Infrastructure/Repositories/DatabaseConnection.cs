using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MicroServices.Interview.Personnel.API.Infrastructure.Repositories
{
    /// <summary>
    /// Bu sınıf ayrı bir core katmana alınıp ileride eklenebilecek diğer micro servislerinde kullanması sağlanabilir
    /// </summary>
    public class DatabaseConnection : Disposable
    {
        #region Constructor

        public DatabaseConnection(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration["ConnectionString"]);
        }

        public DatabaseConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        #endregion Constructor

        #region Properties

        private IDbConnection _connection;

        /// <summary>
        /// Db connection nesnemiz
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
                    _connection.Open();

                return _connection;
            }
        }

        #endregion Properties

        /// <summary>
        /// bağlantı açık ise tüm bağlantıları kapatır
        /// </summary>
        protected override void DisposeCore()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }

            base.DisposeCore();
        }
    }
}