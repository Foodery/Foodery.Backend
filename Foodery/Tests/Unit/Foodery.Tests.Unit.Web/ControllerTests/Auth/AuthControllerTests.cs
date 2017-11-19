using System.Threading.Tasks;
using Foodery.Web.Controllers.Auth;
using Foodery.Web.Models;
using Foodery.Web.Models.Auth.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;
using Foodery.Auth.Interfaces;
using Foodery.Data.Models;
using Foodery.Core.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Foodery.Tests.Unit.Web.ControllerTests.Auth
{
    [TestFixture]
    public class AuthControllerTests
    {
        private const string SamplePassword = "123456q!A";
        private const string SampleUsername = "John Doe";

        [Test]
        public async Task Login_ShouldReturnAppropriateErrorResponse_WhenTheUsernameOrThePasswordIsEmpty()
        {
            // Arrange
            var controller = new AuthController(null, null);
            var loginRequest = new LoginRequest { Password = SamplePassword };

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            this.ValidateErrorResponse(actionResult);

            // Arrange
            loginRequest.Password = null;
            loginRequest.UserName = SampleUsername;

            // Act
            actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            this.ValidateErrorResponse(actionResult);
        }

        [Test]
        public async Task Login_ShouldReturnAppropriateErrorResponse_WhenTheUserDoesntExist()
        {
            // Arrange
            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => null as User));

            var loginRequest = new LoginRequest { Password = SamplePassword, UserName = SampleUsername };
            var controller = new AuthController(userManager.Object, null);

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            this.ValidateErrorResponse(actionResult);
        }

        [Test]
        public async Task Login_ShouldReturnAppropriateErrorResponse_WhenThePasswordDoesntMatch()
        {
            // Arrange
            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => new User()));
            userManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.Run(() => false));

            var loginRequest = new LoginRequest { Password = SamplePassword, UserName = SampleUsername };
            var controller = new AuthController(userManager.Object, null);

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            this.ValidateErrorResponse(actionResult);
        }

        [Test]
        public async Task Login_ShouldReturnAppropriateSuccessResponse_WhenTheLoginIsSuccessful()
        {
            // Arrange
            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => new User()));
            userManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.Run(() => true));

            var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ";
            var tokenProvider = new Mock<ITokenProvider>();
            tokenProvider
                .Setup(x => x.GetJWT(It.IsAny<User>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => jwt));

            var controller = new AuthController(userManager.Object, tokenProvider.Object);
            var loginRequest = new LoginRequest { Password = SamplePassword, UserName = SampleUsername };

            // Act
            var actionResult = await controller.Login(loginRequest) as OkObjectResult;

            // Assert
            var response = this.GetResponse(actionResult);

            // Serialize and then deserialize, because the token property is toLowercase
            // and response.Data cannot be casted to LoginResponse directly.
            var data = JsonConvert.DeserializeObject<LoginResponse>(JsonConvert.SerializeObject(response.Data));
            Assert.IsTrue(response.Success);
            Assert.AreEqual(jwt, data.Token);
        }

        private void ValidateErrorResponse(ObjectResult action)
        {
            var response = this.GetResponse(action);
            Assert.IsFalse(response.Success);
            Assert.IsFalse(string.IsNullOrEmpty(response.Message));
        }

        private DefaultResponse GetResponse(ObjectResult objectResult)
        {
            var response = JsonConvert.DeserializeObject<DefaultResponse>((string)objectResult.Value);
            return response;
        }
    }
}
