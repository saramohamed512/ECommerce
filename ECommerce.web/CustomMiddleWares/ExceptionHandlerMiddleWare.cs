using ECommerce.Services.Execptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;
        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare>logger)
        {
            Next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext Context)
        {
			try
            {
                await Next.Invoke(Context);
                await HandelNotFoundPointAsync(Context);
            }
            catch (Exception ex)
			{

                _logger.LogError(ex, "An unexpected error occurred.");
                var Problem = new ProblemDetails()
                {
                    Title = "An unexpected error occurred!",
                    Detail = ex.Message,
                    Instance= Context.Request.Path,
                    Status = ex switch 
                    { 
                       NotFoundException=> StatusCodes.Status404NotFound,
                          _=> StatusCodes.Status500InternalServerError
                    }
                };
                Context.Response.StatusCode = Problem.Status.Value;

                await Context.Response.WriteAsJsonAsync(Problem);
            }
        }

        private static async Task HandelNotFoundPointAsync(HttpContext Context)
        {
            if (Context.Response.StatusCode == StatusCodes.Status404NotFound && !Context.Response.HasStarted)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "The resource you are looking for is not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The requested resource could not be found on this server.",
                    Instance = Context.Request.Path
                };
                await Context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
