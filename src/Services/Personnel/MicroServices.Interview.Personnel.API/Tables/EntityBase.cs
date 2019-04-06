using MicroServices.Interview.Personnel.API.Model;

namespace MicroServices.Interview.Personnel.API.Tables
{
    /// <summary>
    /// Base model sınıfı bu sınıf içerisinde tüm tablolarda olmasını istediğimiz
    /// örneğin createdDate, UpdatedDate vb. ortak sütunları buradan tanımlayıp her modelde  kullanılması sağlanabilirdi
    /// </summary>
    public class EntityBase : IEntity
    {
    }
}