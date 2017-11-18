using System.Threading.Tasks;
using Foodery.Data.Models;
using Foodery.Web.Models.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodery.Web.Controllers.Auth
{
    public class AccountController : AuthBaseController
    {
        private readonly UserManager<User> userManager;

        public AccountController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.UserName) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return this.BadRequest(InvalidUserNameOrPassword);
            }

            var user = new User { UserName = registerRequest.UserName };
            var passHash = this.userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

            user.PasswordHash = passHash;
            await this.userManager.CreateAsync(user);
            return this.Ok();
        }
    }
}
