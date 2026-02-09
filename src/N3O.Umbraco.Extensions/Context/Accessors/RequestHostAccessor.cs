using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Context;

public class RequestHostAccessor : IRequestHostAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestHostAccessor(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetHost() {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null) {
            return null;
        }

        return ResolveRequestHost(httpContext);
    }

    protected virtual string ResolveRequestHost(HttpContext httpContext) {
        var headerNames = new[] { "X-Forwarded-Host", "X-Original-Host" };
        var header = headerNames.Select(x => httpContext.Request.Headers[x]).FirstOrDefault(x => x.HasValue());
        
        if (header.HasValue()) {
            return header;
        } else {
            return httpContext.Request.Host.Host;
        }
    }
}
