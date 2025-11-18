using Microsoft.AspNetCore.Mvc;

namespace ECommerce.web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(e => e.Key, e => e.Value.Errors

                    .Select(e => e.ErrorMessage).ToArray());
            var ProblemDetails = new ProblemDetails
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,

                Detail = "See the errors property for more details.",
                Extensions =
         {
             { "errors", errors }
         }
            };

            return new BadRequestObjectResult(ProblemDetails);
        }
    }
}
