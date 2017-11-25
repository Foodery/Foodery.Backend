using Foodery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodery.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Create conventional response.
        /// </summary>
        /// <param name="success">Flag that indicates if the operation was successful.</param>
        /// <param name="message">Message for the executed operation.</param>
        /// <param name="data">Any data that should be returned to the client.</param>
        /// <returns>The created response.</returns>
        internal string CreateDefaultResponse(bool success = false, string message = "", object data = null)
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
