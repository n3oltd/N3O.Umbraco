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
            // Content-cache resolution requires an ambient UmbracoContext, which PrePipeline lacks.
            using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                var getPageResult = await _platformsPageAccessor.GetAsync(context.RequestAborted);

                if (getPageResult.HasValue() && getPageResult.IsError) {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    context.Response.Headers.RetryAfter = "10";

                    return;
                }
            }
        } catch (Exception ex) {
            // A CDN failure check must never itself fail the request.
            _logger.Value.LogWarning(ex, "Could not check platforms page for a CDN failure");
        }

        await next(context);
    }
}
