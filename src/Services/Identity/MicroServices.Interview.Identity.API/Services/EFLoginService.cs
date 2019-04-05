using MicroServices.Interview.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MicroServices.Interview.Identity.API.Services
{
    public class EFLoginService : ILoginService<ApplicationUser>
    {
        #region Private variables

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        #endregion Private variables

        #region Constructor

        public EFLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Kullanıcı adına göre user bilgisi çeker
        /// </summary>
        /// <param name="email">Eposta adresi</param>
        /// <returns>Kullanıcı nesnesi</returns>
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Kullanıcı login kontrol
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <param name="password">şifresi</param>
        /// <returns></returns>
        public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task SignIn(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        #endregion Methods
    }
}