using System.Collections.Generic;
using System.Linq;
using Foodery.Web.Controllers;
using Foodery.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Foodery.Web.Attributes.Filters.Action
{
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Validates the model state. If there are errors, they will be extracted
        /// and returned with the response.
        /// </summary>
        /// <param name="context">Execution context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = this.GetModelStateErrors(context.ModelState);
                var controller = context.Controller as BaseController;
                context.Result = new BadRequestObjectResult(controller.CreateDefaultResponse(success: false, data: errors));
            }
        }

        /// <summary>
        /// Extract the errors from the model state as collection of { Field, Message }. 
        /// </summary>
        /// <returns>Collection of the extracted model state errors.</returns>
        private IEnumerable<ValidationError> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = modelState
                             .Keys
                             .SelectMany(key => modelState[key].Errors.Select(e => new ValidationError { Field = key, Message = e.ErrorMessage }))
                             .ToList();
            return errors;
        }
    }
}
