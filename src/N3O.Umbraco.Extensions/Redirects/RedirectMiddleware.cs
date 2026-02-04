using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Redirects;

public class RedirectMiddleware : IMiddleware {
    private readonly IRedirectManagement _redirectManagement;
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    
    public RedirectMiddleware(IRedirectManagement redirectManagement, Lazy<IUmbracoContextFactory> umbracoContextFactory) {
        _redirectManagement = redirectManagement;
        _umbracoContextFactory = umbracoContextFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var originalBodyStream = context.Response.Body;
        
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try {
            await next(context);

            if (context.Response.StatusCode == StatusCodes.Status404NotFound) {
                using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                    var path = context.Request.Path.Value;

                    if (path.HasValue()) {
                        if (TryRedirect(context, path)) {
                            return;
                        }
                    }
                }
            }
            
            if (responseBody.Length > 0) {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        } finally {
            context.Response.Body = originalBodyStream;
        } 
    }
    
    private bool TryRedirect(HttpContext context, string path) {
        var redirect = _redirectManagement.FindRedirect(path);

        if (redirect != null) {
            _redirectManagement.LogHit(redirect.Id);

            Redirect(context, redirect.Temporary, redirect.Url);

            return true;
        }
        
        var staticRedirect = StaticRedirects.Find(path);

        if (staticRedirect != null) {
            Redirect(context, staticRedirect.Temporary, staticRedirect.Path);

            return true;
        }

        return false;
    }
    
    private void Redirect(HttpContext context, bool temporary, string urlOrPath) {
        context.Response.Clear();
        context.Response.Redirect(urlOrPath, permanent: !temporary);
    }
}