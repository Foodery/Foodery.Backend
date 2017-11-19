using System.Threading.Tasks;
using Foodery.Web.Controllers.Auth;
using Foodery.Web.Models.Auth.Login;
using Foodery.Auth.Interfaces;
using Foodery.Data.Models;
using Foodery.Common.Validation.Constants;
using Foodery.Core.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Moq;

namespace Foodery.Tests.Unit.Web.ControllerTests.Auth
{
    [TestFixture]
    public class AuthControllerTests
    {
        [Test]
        public async Task Login_ShouldReturnAppropriateErrorResponse_WhenTheUsernameOrThePasswordIsEmpty()
        {
            // Arrange
            var controller = new AuthController(null, null);
            var loginRequest = new LoginRequest { Password = AuthCommon.SamplePassword };

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);

            // Arrange
            loginRequest.Password = null;
            loginRequest.UserName = AuthCommon.SampleUsername;

            // Act
            actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);
        }

        [Test]
        public async Task Login_ShouldReturnAppropriateErrorResponse_WhenTheUserDoesntExist()
        {
            // Arrange
            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => null as User));

            var loginRequest = new LoginRequest { Password = AuthCommon.SamplePassword, UserName = AuthCommon.SampleUsername };
            var controller = new AuthController(userManager.Object, null);

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);
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

            var loginRequest = new LoginRequest { Password = AuthCommon.SamplePassword, UserName = AuthCommon.SampleUsername };
            var controller = new AuthController(userManager.Object, null);

            // Act
            var actionResult = await controller.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);
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
            var loginRequest = new LoginRequest { Password = AuthCommon.SamplePassword, UserName = AuthCommon.SampleUsername };

            // Act
            var actionResult = await controller.Login(loginRequest) as OkObjectResult;

            // Assert
            var response = AuthCommon.GetResponse(actionResult);            
            var data = AuthCommon.GetNormalizedDataObject<LoginResponse>(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(jwt, data.Token);
        }
    }
}
