using Foodery.Web.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foodery.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register([FromBody] RegisterRequestModel registerRequestModel)
        {
            return this.Json(registerRequestModel);
        }
    }
}
