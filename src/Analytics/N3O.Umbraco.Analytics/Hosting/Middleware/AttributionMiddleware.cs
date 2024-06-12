using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Analytics.Services;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Analytics.Hosting;

public class AttributionMiddleware : IMiddleware {
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    private readonly IAttributionHelper _attributionHelper;
    
    public AttributionMiddleware(Lazy<IUmbracoContextFactory> umbracoContextFactory, IAttributionHelper attributionHelper) {
        _umbracoContextFactory = umbracoContextFactory;
        _attributionHelper = attributionHelper;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        if (_attributionHelper.HasAttributions(context.Request.Query)) {
            using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                _attributionHelper.AddOrUpdateAttributionCookie(context);
            }
        }

        await next(context);
    }
}