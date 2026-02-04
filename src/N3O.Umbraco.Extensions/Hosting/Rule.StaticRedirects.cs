using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace N3O.Umbraco.Hosting;

public class StaticRedirectsRule : IRule {
    public void ApplyRule(RewriteContext context) {
        var requestedPath = $"{context.HttpContext.Request.PathBase}{context.HttpContext.Request.Path}";
        
        var staticRedirect = StaticRedirects.Find(requestedPath);

        if (staticRedirect != null) {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.PermanentRedirect;
            context.HttpContext.Response.Headers[HeaderNames.Location] = staticRedirect.Path;
            
            context.Result = RuleResult.EndResponse;
        }
    }
}
