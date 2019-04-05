using System.Threading.Tasks;

namespace MicroServices.Interview.Identity.API.Services
{
    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);

        Task<T> FindByEmailAsync(string email);

        Task SignIn(T user);
    }
}