using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestExample.Infrastructure
{
    /// <summary>
    /// Provides support for <see cref="IHeaderDictionary.LastModified"/> and <see cref="IHeaderDictionary.IfModifiedSince"/>
    /// when used with a successful returned <see cref="Entity"/>.
    /// </summary>
    public class LastModifiedEntityActionFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            var requestHeaders = executingContext.HttpContext.Request.Headers;
            var executedContext = await next();

            if (executedContext.Result is ObjectResult result && IsSuccessStatusCode(result.StatusCode) && result.Value is Entity entity)
            {
                var headers = executedContext.HttpContext.Response.Headers;
                headers.LastModified = entity.UpdatedAt.ToString("R");

                if (DateTime.TryParse(requestHeaders.IfModifiedSince.ToString(), out var lastModifiedSince))
                {
                    if (lastModifiedSince <= entity.UpdatedAt)
                        executedContext.Result = NotModifiedStatusCode.Instance;
                }
            }
        }

        static bool IsSuccessStatusCode(int? code)
        {
            return code != null && code.Value >= 200 && code.Value < 300;
        }
    }
}
