using Microsoft.AspNetCore.Rewrite;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Hosting;

[Order(0)]
public class CanonicalDomainRule : IRule {
    private readonly string _canonicalDomain;
    private readonly IReadOnlyList<string> _aliasDomains;

    public CanonicalDomainRule() {
        _canonicalDomain = EnvironmentData.GetOurValue(HostingConstants.Environment.Keys.CanonicalDomain);
        _aliasDomains = EnvironmentData.GetOurValue(HostingConstants.Environment.Keys.AliasDomains).Or("").Split('|');
    }
    
    public void ApplyRule(RewriteContext context) {
        var host = context.HttpContext.Request.Host.Host;

        if (_canonicalDomain.HasValue() &&
            !context.HttpContext.Request.IsLocal() &&
            !host.EqualsInvariant(_canonicalDomain) &&
            !_aliasDomains.Contains(host, true)) {
            var req = context.HttpContext.Request;
            var newUrl = $"{req.Scheme}://{_canonicalDomain}{req.PathBase}{req.Path}{req.QueryString}";
            
            context.HttpContext.Response.Redirect(newUrl, true);
            
            context.Result = RuleResult.EndResponse;
        }
    }
}
