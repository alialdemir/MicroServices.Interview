using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MicroServices.Interview.Identity.API.Configuration
{
    public class Config
    {
        #region Methods

        /// <summary>
        /// Identity üzerinden hangi micro servisler haberleşebilir
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("personnel", "Personnel Service"),
            };
        }

        /// <summary>
        /// Kullanıcı login olunca hangi kaynaklara erişebilirler
        /// örneğin ad, soyad, email
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// Client side tarafından login olan kullanıcı hangi micro servislere erişme hakkı olsun.
        /// mesela mobil ve web clientimiz olsun, web tarafından personel servisinde işlem yapabilsin ama
        /// mobileden girenler erişemesin mobilde x,y,z servislerine erişsin yapılabilir
        /// webten x,y,z servislerine istek gitsede başarısız olur...
        /// facebook, twitter api kullandıysanız oradaki scopes mantığını uygular
        /// </summary>
        /// <returns>List of clients</returns>
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {  new Client
                        {
                            ClientId = "angular",
                            ClientName = "Angular Client",
                            AccessTokenLifetime = 3600 * 24 * 90,// 3 month
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            RequireClientSecret = false,
                            AllowedScopes =new List<string>
                                    {
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.OfflineAccess,
                                        "personnel"
                                    },
                        },
            };
        }

        #endregion Methods
    }
}