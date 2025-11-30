using ECommerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        /// Comman Result 
        /// Handle Result

        // Hnalde request without values:-
        // iF Result Success=>return No COntent [204]

        // if Result Failure=> Resurn Problem DEtails with Status Code , ErrorDetails

        // Hnalde request with values:-
        // if Result Success=> return Value with Ok [200]
        // if Result Failure=> Resurn Problem DEtails with Status Code , ErrorDetails

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                var firstError = result.Errors.First();
                return HandleProblem(result.Errors);
            }
        }
        protected string GetEmailFromToken()
         => User.FindFirstValue(ClaimTypes.Email)!;
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value); // 200 OK with Value
            }
            else
            {
                return HandleProblem(result.Errors);
            }
        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no errors => Return Default Error 500
            // if there is only one error => handle it
            // if there is more than one error => Handle it as Validation Error

            if (errors.Count == 0)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal Server Error", detail: "UnExcepted Error Occured !");

            }
            if (errors.All(E => E.Type == ErrorType.Validation))
            {
                return HandleValidationProblem(errors);
            }

            return HandelSingleErrorProblem(errors[0]);
        }


        private ActionResult HandelSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type));

        }
        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            // Modle State
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }



    }
}
