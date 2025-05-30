using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System.Net;
using System.Text;

namespace Presentation
{
    internal class RedisCacheAttribute(int durationInSec)
        : ActionFilterAttribute
    {



        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            string cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var result = await cacheService.GetCachedItem(cacheKey);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "Application/Json",
                    StatusCode = (int)HttpStatusCode.OK
                };

                return;
            }
            var contextResult = await next.Invoke();

            if (contextResult.Result is OkObjectResult okObject)
            {
                await cacheService.SetCacheValue(cacheKey, okObject, TimeSpan.FromSeconds(durationInSec));
            }



        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);

            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }

            return keyBuilder.ToString();
        }
    }
}
// api/Products|PageIndex-1|PageSize-7
// ? PageIndex = 1 & PageSize = 7 & search = Chicken