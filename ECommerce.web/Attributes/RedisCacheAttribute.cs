using Microsoft.AspNetCore.Mvc.Filters;
namespace ECommerce.web.Attributes
{
    public class RedisCacheAttribute:ActionFilterAttribute
    {
     
        //async execute after action method
         public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
