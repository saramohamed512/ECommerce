using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {

        private readonly int _durationInMin;
        public RedisCacheAttribute(int DurationInMin = 5)
        {
            _durationInMin = DurationInMin;
        }
        //async execute after action method
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //get cache service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            //check if cache exist
            //create cashe key
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var cacheValue = await cacheService.GetAsyn(cacheKey);
            if (cacheValue is not null)
            {
                //return cache value
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result)
            {
                //set cache
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(5));
            }

        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }
            return Key.ToString();

        }
    }
}
