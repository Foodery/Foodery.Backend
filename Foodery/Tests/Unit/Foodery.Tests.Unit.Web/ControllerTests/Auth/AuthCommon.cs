using Foodery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Foodery.Tests.Unit.Web.ControllerTests.Auth
{
    internal class AuthCommon
    {
        internal const string SamplePassword = "123456q!A";
        internal const string SampleUsername = "John Doe";

        internal static void ValidateErrorResponse(ObjectResult action, string expectedMessage = null)
        {
            var response = GetResponse(action);
            Assert.IsFalse(response.Success);
            Assert.IsFalse(string.IsNullOrEmpty(response.Message));

            if (expectedMessage != null)
            {
                Assert.AreEqual(expectedMessage, response.Message);
            }
        }

        internal static DefaultResponse GetResponse(ObjectResult objectResult)
        {
            var response = JsonConvert.DeserializeObject<DefaultResponse>((string)objectResult.Value);
            return response;
        }

        internal static T GetNormalizedDataObject<T>(DefaultResponse response)
            where T: class
        {
            // Serialize and then deserialize, because the token property is toLowercase
            // and response.Data cannot be casted to T directly.
            var dataObj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response.Data));
            return dataObj;
        }
    }
}
