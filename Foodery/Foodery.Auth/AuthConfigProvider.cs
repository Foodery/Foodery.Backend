using System.Collections.Generic;
using Foodery.Core.Auth.Interfaces;
using IdentityServer4;
using IdentityServer4.Models;

namespace Foodery.Auth
{
    public class AuthConfigProvider : IAuthConfigProvider
    {
        /// <summary>
        /// Get all api resources.
        /// </summary>
        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("foodery_auth_api", "Foodery.Security.Api")
            };
        }

        /// <summary>
        /// Get all identity resources such as Profile etc.
        /// </summary>
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// Get the clients that can be authenticated.
        /// </summary>
        public IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // TODO: refactor this client
                new Client
                {
                    ClientId = "foodery_auth_client",
                    ClientName = "Foodery.Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "foodery_auth_api"
                    },
                }
            };
        }
    }
}
