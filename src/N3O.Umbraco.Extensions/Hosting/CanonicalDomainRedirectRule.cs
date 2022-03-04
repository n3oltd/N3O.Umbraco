using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using N3O.Umbraco.Extensions;
using System.Net;

namespace N3O.Umbraco.Hosting {
    public class CanonicalDomainRedirectRule : IRule {
        private readonly string _canonicalDomain;

        public CanonicalDomainRedirectRule(string canonicalDomain) {
            _canonicalDomain = canonicalDomain;
        }
        
        public void ApplyRule(RewriteContext context) {
            if (!context.HttpContext.Request.Host.Host.EqualsInvariant(_canonicalDomain)) {
                var req = context.HttpContext.Request;
                var newUrl = $"{req.Scheme}://{_canonicalDomain}{req.PathBase}{req.Path}{req.QueryString}";
                
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.PermanentRedirect;
                context.HttpContext.Response.Headers[HeaderNames.Location] = newUrl;
                
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}