using System.Collections.Generic;

namespace MicroServices.Interview.Personnel.API.Infrastructure.Repositories
{
    public interface IPersonnelRepository : IRepository<Tables.Personnel>
    {
        IEnumerable<Model.Personnel> GetAllPersonnels();
    }
}