using Foodery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Foodery.Tests.Unit.Web.Utils
{
    internal class ObjectResultUtils
    {
        internal static DefaultResponse GetResponse(ObjectResult objectResult)
        {
            var response = JsonConvert.DeserializeObject<DefaultResponse>((string)objectResult.Value);
            return response;
        }

        internal static T GetNormalizedDataObject<T>(DefaultResponse response)
            where T : class
        {
            // Serialize and then deserialize, because the token property is toLowercase
            // and response.Data cannot be casted to T directly.
            var dataObj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response.Data));
            return dataObj;
        }
    }
}
