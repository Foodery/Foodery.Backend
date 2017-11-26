using System.ComponentModel.DataAnnotations;

namespace Foodery.Web.Models.Auth.Login
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
