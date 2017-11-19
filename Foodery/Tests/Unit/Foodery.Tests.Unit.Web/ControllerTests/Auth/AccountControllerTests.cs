using System.Threading.Tasks;
using AutoMapper;
using Foodery.Auth.Interfaces;
using Foodery.Common.Validation.Constants;
using Foodery.Data.Models;
using Foodery.Web.Controllers.Auth;
using Foodery.Web.Models.Auth.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Foodery.Tests.Unit.Web.ControllerTests.Auth
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public async Task Register_ShouldReturnAppropriateErrorResponse_WhenUsernameOrPasswordIsMissing()
        {
            // Arrange
            var controller = new AccountController(null, null);
            var registerRequest = new RegisterRequest { Password = AuthCommon.SamplePassword };

            // Act
            var actionResult = await controller.Register(registerRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);

            // Arrange
            registerRequest.Password = null;
            registerRequest.UserName = AuthCommon.SampleUsername;

            // Act
            actionResult = await controller.Register(registerRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.InvalidUserNameOrPassword);
        }

        [Test]
        public async Task Register_ShouldReturnAppropriateErrorResponse_WhenUserWithSuchUsernameAlreadyExists()
        {
            // Arrange
            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => new User()));

            var controller = new AccountController(userManager.Object, null);
            var registerRequest = new RegisterRequest
            {
                Password = AuthCommon.SamplePassword,
                UserName = AuthCommon.SampleUsername
            };

            // Act
            var actionResult = await controller.Register(registerRequest) as BadRequestObjectResult;

            // Assert
            AuthCommon.ValidateErrorResponse(actionResult, UserConstants.ValidationMessages.UserAlreadyExists);
        }

        [Test]
        public async Task Register_ShouldReturnAppropriateSuccessResponse_WhenTheRegistrationIsSuccessful()
        {
            // Arrange
            var hashedPassword = "946d040b406619977a5628a7ba02043589e5bc6dadb2ce2b97ca329dc64a4be0";
            var registerRequest = new RegisterRequest
            {
                Password = AuthCommon.SamplePassword,
                UserName = AuthCommon.SampleUsername
            };

            var passwordHasher = new Mock<IPasswordHasher<User>>();
            passwordHasher
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.Is<string>(p => p == registerRequest.Password)))
                .Returns(hashedPassword);

            var userManager = new Mock<IApplicationUserManager>();
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => (User)null));
            userManager
                .Setup(x => x.CreateAsync(It.Is<User>(u => u.UserName == registerRequest.UserName)))
                .Returns(Task.Run(() => (IdentityResult)null));
            userManager
                .SetupGet(x => x.PasswordHasher)
                .Returns(passwordHasher.Object);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<RegisterResponse>(It.Is<User>(u => u.PasswordHash == hashedPassword)))
                .Returns((User user) =>
                {
                    return new RegisterResponse
                    {
                        UserName = registerRequest.UserName,
                        Id = user.Id
                    };
                });

            var controller = new AccountController(userManager.Object, mapper.Object);

            // Act
            var actionResult = await controller.Register(registerRequest) as OkObjectResult;

            // Assert
            var response = AuthCommon.GetResponse(actionResult);
            var data = AuthCommon.GetNormalizedDataObject<RegisterResponse>(response);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(registerRequest.UserName, data.UserName);
        }
    }
}
