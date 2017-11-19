using System.Threading.Tasks;
using Foodery.Data.Models;
using Foodery.Web.Models.Auth.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Foodery.Web.Controllers.Auth
{
    public class AccountController : AuthBaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<User> userManager,
                                 IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Register new user by given user data.
        /// </summary>
        /// <param name="registerRequest">User data.</param>
        /// <returns>OK if the registration is sucessful or BAD_REQUEST if the user data is not valid.</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.UserName) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return this.BadRequest(this.CreateDefaultResponse(message: InvalidUserNameOrPassword));
            }

            var foundUser = await this.userManager.FindByNameAsync(registerRequest.UserName);
            if (foundUser != null)
            {
                return this.BadRequest(this.CreateDefaultResponse(message: UserAlreadyExists));
            }

            var user = new User { UserName = registerRequest.UserName };
            var passHash = this.userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

            user.PasswordHash = passHash;
            await this.userManager.CreateAsync(user);

            var data = this.mapper.Map<RegisterResponse>(user);
            return this.Ok(this.CreateDefaultResponse(success: true, data: data));
        }
    }
}
