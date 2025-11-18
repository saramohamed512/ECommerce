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
                if( Context.Response.StatusCode == StatusCodes.Status404NotFound)
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
			catch (Exception ex)
			{

                _logger.LogError(ex, "An unexpected error occurred.");
                Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var Problem = new ProblemDetails()
                {
                    Title = "An unexpected error occurred!",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Instance= Context.Request.Path
                };
                await Context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
