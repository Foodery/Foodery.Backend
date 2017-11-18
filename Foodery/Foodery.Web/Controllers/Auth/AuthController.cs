using System;
using System.Linq;
using System.Threading.Tasks;
using Foodery.Data.Models;
using Foodery.Web.Config;
using Foodery.Web.Models.Login;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodery.Web.Controllers.Auth
{
    public class AuthController : AuthBaseController
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<User> userManager;
        private readonly IUserClaimsPrincipalFactory<User> principalFactory;
        private readonly IdentityServerOptions identityServerOptions;

        public AuthController(
                              ITokenService tokenService,
                              UserManager<User> userManager,
                              IUserClaimsPrincipalFactory<User> principalFactory,
                              IdentityServerOptions identityServerOptions)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.principalFactory = principalFactory;
            this.identityServerOptions = identityServerOptions;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return this.BadRequest(InvalidUserNameOrPassword);
            }

            var foundUser = await this.userManager.FindByNameAsync(loginRequest.UserName);
            if (foundUser == null)
            {
                return this.BadRequest(UserAlreadyExists);
            }

            bool hasValidPassword = await this.userManager.CheckPasswordAsync(foundUser, loginRequest.Password);
            if (!hasValidPassword)
            {
                return this.BadRequest(InvalidUserNameOrPassword);
            }

            var token = await this.GetToken(foundUser);
            return this.Json(new LoginResponse { Token = token });
        }

        private async Task<string> GetToken(User user)
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
                Resources = new Resources(AuthConfig.GetIdentityResources(), AuthConfig.GetApiResources()),
                ValidatedRequest = new ValidatedRequest
                {
                    Subject = subject,
                    Options = this.identityServerOptions,
                    ClientClaims = identityUser.AdditionalClaims,

                    // TODO: determine which client exactly should be used
                    Client = AuthConfig.GetClients().First()
                },
            };

            var accessToken = await this.tokenService.CreateAccessTokenAsync(tokenRequest);
            accessToken.Issuer = this.HttpContext.Request.Scheme + "://" + this.HttpContext.Request.Host.Value;

            // TODO: add proper lifetime
            accessToken.Lifetime = 3600;

            var securityToken = await this.tokenService.CreateSecurityTokenAsync(accessToken);
            return securityToken;
        }
    }
}
