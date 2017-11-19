using Foodery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodery.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string CreateDefaultResponse(bool success = false, string message = "", object data = null)
        {
            var response = new DefaultResponse
            {
                Data = data,
                Message = message,
                Success = success
            };
            var responseJson = JsonConvert.SerializeObject(response);

            return responseJson;
        }
    }
}
