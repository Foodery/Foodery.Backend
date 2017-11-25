using System.Threading.Tasks;
using Foodery.Web.Models.Auth.Login;
using Foodery.Core.Auth.Interfaces;
using Foodery.Auth.Interfaces;
using Foodery.Common.Validation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Foodery.Web.Controllers.Auth
{
    /// <summary>
    /// Manage different types of login and logout.
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly IApplicationUserManager userManager;
        private readonly ITokenProvider tokenProvider;

        public AuthController(IApplicationUserManager userManager,
                              ITokenProvider tokenProvider)
        {
            this.userManager = userManager;
            this.tokenProvider = tokenProvider;
        }

        /// <summary>
        /// By given user data, try to login the user.
        /// If the login is successful, JWT will be returned.
        /// </summary>
        /// <param name="loginRequest">Data that is used for the login.</param>
        /// <returns>If the login is successful, return the JWT, if not, 
        /// returns apropriate error response.</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var foundUser = await this.userManager.FindByNameAsync(loginRequest.UserName);
            if (foundUser == null)
            {
                return this.BadRequest(this.CreateDefaultResponse(message: UserConstants.ValidationMessages.InvalidUserNameOrPassword));
            }

            bool hasValidPassword = await this.userManager.CheckPasswordAsync(foundUser, loginRequest.Password);
            if (!hasValidPassword)
            {
                return this.BadRequest(this.CreateDefaultResponse(message: UserConstants.ValidationMessages.InvalidUserNameOrPassword));
            }

            var token = await this.tokenProvider.GetJWT(foundUser, this.HttpContext);
            return this.Ok(this.CreateDefaultResponse(success: true, data: new LoginResponse { Token = token }));
        }
    }
}
