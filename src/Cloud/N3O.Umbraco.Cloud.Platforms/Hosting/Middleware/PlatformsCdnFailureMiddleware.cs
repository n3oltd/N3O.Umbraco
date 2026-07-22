using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Hosting;

public class PlatformsCdnFailureMiddleware : IMiddleware {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    public PlatformsCdnFailureMiddleware(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var getPageResult = await _platformsPageAccessor.GetAsync(context.RequestAborted);

        if (getPageResult.HasValue() && getPageResult.IsError) {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.Headers.RetryAfter = "10";

            return;
        }

        await next(context);
    }
}