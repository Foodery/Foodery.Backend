using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Foodery.Tests.Unit.Web.Utils
{
    internal class ActionContextUtils
    {
        internal static ActionExecutingContext MockActionExecutingContext(HttpContext context, object controller, ModelStateDictionary modelState)
        {
            var actionContext = new ActionContext(
                context,
                new Mock<RouteData>().Object,
                new Mock<ActionDescriptor>().Object,
                modelState
            );
            var filters = new List<IFilterMetadata>();
            var actionArguments = new Dictionary<string, object>();
            var executingContext = new ActionExecutingContext(actionContext, filters, actionArguments, controller);
            return executingContext;
        }
    }
}
