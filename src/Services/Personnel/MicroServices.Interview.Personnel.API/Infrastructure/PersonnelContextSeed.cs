using MicroServices.Interview.Personnel.API.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.Interview.Personnel.API.Infrastructure
{
    public class PersonnelContextSeed
    {
        #region Constructor

        public PersonnelContextSeed(string connectionString, ILogger<PersonnelContextSeed> logger)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructor

        #region Properties

        private string ConnectionString { get; set; }

        private ILogger<PersonnelContextSeed> Logger { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// personel tablosuna default kayıtları ekledik
        /// </summary>
        /// <returns></returns>
        public async Task InsertDataAsync()
        {
            Logger.LogInformation($"The entities are being added. {typeof(Model.Personnel).Name}");

            DapperRepositoryBase<Model.Personnel> repository = new DapperRepositoryBase<Model.Personnel>(ConnectionString);

            if (repository.GetCount() == 0)// tekrar tekrar eklemesin diye kayıt yoksa ekledik
            {
                await repository.InsertAsync(GetPersonnels());
            }

            Logger.LogInformation($"The entities have been added.. {typeof(Model.Personnel).Name}");
        }

        private IEnumerable<Model.Personnel> GetPersonnels()
        {
            return new List<Model.Personnel>
            {
                new Model.Personnel{ FullName="Ali Aldemir",  Age=26, City = "İstanbul"},
                new Model.Personnel{ FullName="Ayşe Gül",  Age=23, City = "Ankara"},
                new Model.Personnel{ FullName="Kadir Çöp",  Age=  19, City = "Bursa"},
            };
        }

        #endregion Methods
    }
}