using Foodery.Tests.Unit.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Foodery.Tests.Unit.Web.ControllerTests.Auth
{
    internal class AuthCommon
    {
        internal const string SamplePassword = "123456q!A";
        internal const string SampleUsername = "John Doe";

        internal static void ValidateErrorResponse(ObjectResult action, string expectedMessage = null)
        {
            var response = ObjectResultUtils.GetResponse(action);
            Assert.IsFalse(response.Success);
            Assert.IsFalse(string.IsNullOrEmpty(response.Message));

            if (expectedMessage != null)
            {
                Assert.AreEqual(expectedMessage, response.Message);
            }
        }
    }
}
