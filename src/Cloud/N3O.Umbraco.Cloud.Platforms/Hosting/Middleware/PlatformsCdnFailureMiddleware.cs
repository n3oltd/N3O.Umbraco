using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Cloud.Platforms.Hosting;

public class PlatformsCdnFailureMiddleware : IMiddleware {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    private readonly Lazy<ILogger<PlatformsCdnFailureMiddleware>> _logger;

    public PlatformsCdnFailureMiddleware(IPlatformsPageAccessor platformsPageAccessor,
                                         Lazy<IUmbracoContextFactory> umbracoContextFactory,
                                         Lazy<ILogger<PlatformsCdnFailureMiddleware>> logger) {
        _platformsPageAccessor = platformsPageAccessor;
        _umbracoContextFactory = umbracoContextFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            // Page resolution reads the content cache, which requires an ambient UmbracoContext;
            // one does not yet exist this early in the pipeline, so establish it here.
            using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                var getPageResult = await _platformsPageAccessor.GetAsync(context.RequestAborted);

                if (getPageResult.HasValue() && getPageResult.IsError) {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    context.Response.Headers.RetryAfter = "10";

                    return;
                }
            }
        } catch (Exception ex) {
            // A CDN failure check must never itself fail the request; fall through to the pipeline.
            _logger.Value.LogWarning(ex, "Could not check platforms page for a CDN failure");
        }

        await next(context);
    }
}
