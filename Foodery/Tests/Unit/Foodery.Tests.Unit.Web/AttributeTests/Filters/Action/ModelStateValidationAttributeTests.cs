using System.Collections.Generic;
using System.Linq;
using Foodery.Tests.Unit.Web.Utils;
using Foodery.Web.Attributes.Filters.Action;
using Foodery.Web.Controllers;
using Foodery.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;

namespace Foodery.Tests.Unit.Web.AttributeTests.Filters.Action
{
    [TestFixture]
    public class ModelStateValidationAttributeTests
    {
        [Test]
        public void OnActionExecuting_WhenTheModelStateIsNotValid_ShouldReturnCorrectResponse()
        {
            // Arrange
            var userNameProperty = "UserName";
            var userNameMessage = "Invalid UserName.";
            var passwordProperty = "Password";
            var passwordMessage = "Invalid Password.";

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(userNameProperty, userNameMessage);
            modelState.AddModelError(passwordProperty, passwordMessage);

            var httpContext = new Mock<HttpContext>().Object;
            var controller = new BaseController();
            var actionExecutingContext = ActionContextUtils.MockActionExecutingContext(httpContext, controller, modelState);
            var modelStateValidationAttribute = new ModelStateValidationAttribute();

            // Act
            modelStateValidationAttribute.OnActionExecuting(actionExecutingContext);
            var result = actionExecutingContext.Result as BadRequestObjectResult;
            var response = ObjectResultUtils.GetResponse(result);
            var errors = ObjectResultUtils.GetNormalizedDataObject<List<ValidationError>>(response);

            // Assert
            Assert.AreEqual(modelState.ErrorCount, errors.Count);

            var userNameError = errors.First(e => e.Field == userNameProperty);
            Assert.AreEqual(userNameMessage, userNameError.Message);

            var passwordError = errors.First(e => e.Field == passwordProperty);
            Assert.AreEqual(passwordMessage, passwordError.Message);
        }
    }
}
