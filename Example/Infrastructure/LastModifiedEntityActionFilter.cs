using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestExample.Infrastructure
{
    public class LastModifiedEntityActionFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            var requestHeaders = executingContext.HttpContext.Request.Headers;
            var executedContext = await next();

            if (DateTime.TryParse(requestHeaders.IfModifiedSince.ToString(), out var lastModifiedSince))
            {
                if (executedContext.Result is ObjectResult result && IsSuccessStatusCode(result.StatusCode) && result.Value is Entity entity)
                {
                    if (lastModifiedSince <= entity.UpdatedAt)
                        executedContext.Result = NotModifiedStatusCode.Instance;

                    var headers = executedContext.HttpContext.Response.Headers;
                    headers.LastModified = entity.UpdatedAt.ToString("R");
                }
            }
        }

        private static bool IsSuccessStatusCode(int? code)
        {
            return code != null && code.Value >= 200 && code.Value < 300;
        }
    }
}
