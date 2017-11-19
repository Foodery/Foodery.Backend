using System;
using System.Linq;
using System.Threading.Tasks;
using Foodery.Core.Auth.Interfaces;
using Foodery.Data.Models;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Foodery.Auth
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ITokenService tokenService;
        private readonly IUserClaimsPrincipalFactory<User> principalFactory;
        private readonly IdentityServerOptions identityServerOptions;
        private readonly IAuthConfigProvider authConfigProvider;

        public TokenProvider(
                              ITokenService tokenService,
                              IUserClaimsPrincipalFactory<User> principalFactory,
                              IdentityServerOptions identityServerOptions,
                              IAuthConfigProvider authConfigProvider)
        {
            this.tokenService = tokenService;
            this.principalFactory = principalFactory;
            this.identityServerOptions = identityServerOptions;
            this.authConfigProvider = authConfigProvider;
        }

        /// <summary>
        /// Setup and create JWT by given user data.
        /// </summary>
        /// <param name="user">User data used for the generation of JWT.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>JWT.</returns>
        public async Task<string> GetJWT(User user, HttpContext httpContext)
        {
            var identityPrincipal = await this.principalFactory.CreateAsync(user);
            var identityUser = new IdentityServerUser(user.Id.ToString())
            {
                AdditionalClaims = identityPrincipal.Claims.ToArray(),
                DisplayName = user.UserName,
                AuthenticationTime = DateTime.UtcNow,
                IdentityProvider = IdentityServerConstants.LocalIdentityProvider
            };
            var subject = identityUser.CreatePrincipal();
            var tokenRequest = new TokenCreationRequest
            {
                Subject = subject,
                IncludeAllIdentityClaims = true,
                Resources = new Resources(this.authConfigProvider.GetIdentityResources(), this.authConfigProvider.GetApiResources()),
                ValidatedRequest = new ValidatedRequest
                {
                    Subject = subject,
                    Options = this.identityServerOptions,
                    ClientClaims = identityUser.AdditionalClaims,

                    // TODO: determine which client exactly should be used
                    Client = this.authConfigProvider.GetClients().First()
                },
            };

            var accessToken = await this.tokenService.CreateAccessTokenAsync(tokenRequest);
            accessToken.Issuer = httpContext.Request.Scheme + "://" + httpContext.Request.Host.Value;

            // TODO: add proper lifetime
            accessToken.Lifetime = 3600;

            var securityToken = await this.tokenService.CreateSecurityTokenAsync(accessToken);
            return securityToken;
        }
    }
}
