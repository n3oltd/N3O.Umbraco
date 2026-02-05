using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Redirects;

public class RedirectMiddleware : IMiddleware {
    private readonly Lazy<IRedirectManagement> _redirectManagement;
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    
    public RedirectMiddleware(Lazy<IRedirectManagement> redirectManagement,
                              Lazy<IUmbracoContextFactory> umbracoContextFactory) {
        _redirectManagement = redirectManagement;
        _umbracoContextFactory = umbracoContextFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var originalBodyStream = context.Response.Body;

        await using (var responseBody = new MemoryStream()) {
            context.Response.Body = responseBody;

            try {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound) {
                    using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                        var path = context.Request.Path.Value;

                        if (path.HasValue()) {
                            if (TryRedirect(context.Response, path)) {
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
    }
    
    private bool TryRedirect(HttpResponse response, string path) {
        var redirect = _redirectManagement.Value.FindRedirect(path);
        
        if (redirect != null) {
            redirect = StaticRedirects.Find(path);
        }

        if (redirect != null) {
            response.ContentLength = 0;
            response.Redirect(redirect.UrlOrPath, permanent: !redirect.Temporary);

            return true;
        } else {
            return false;
        }
    }
}