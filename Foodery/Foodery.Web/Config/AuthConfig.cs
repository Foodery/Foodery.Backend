using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Foodery.Web.Config
{
    public static class AuthConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("foodery_auth_api", "Foodery.Security.Api")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
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
