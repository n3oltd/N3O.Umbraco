using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Umbraco.Extensions;

namespace N3O.Umbraco.Hosting;

public class CanonicalDomainRedirectRule : IRule {
    private readonly string _canonicalDomain;
    private readonly IReadOnlyList<string> _aliasDomains;

    public CanonicalDomainRedirectRule(string canonicalDomain, IEnumerable<string> aliasDomains) {
        _canonicalDomain = canonicalDomain;
        _aliasDomains = aliasDomains.OrEmpty().Select(x => x?.Trim()).Where(x => x.HasValue()).ToList();
    }
    
    public void ApplyRule(RewriteContext context) {
        var host = context.HttpContext.Request.Host.Host;

        if (!context.HttpContext.Request.IsLocal() &&
            !host.EqualsInvariant(_canonicalDomain) &&
            !_aliasDomains.Contains(host, true)) {
            var req = context.HttpContext.Request;
            var newUrl = $"{req.Scheme}://{_canonicalDomain}{req.PathBase}{req.Path}{req.QueryString}";
            
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.PermanentRedirect;
            context.HttpContext.Response.Headers[HeaderNames.Location] = newUrl;
            
            context.Result = RuleResult.EndResponse;
        }
    }
}
