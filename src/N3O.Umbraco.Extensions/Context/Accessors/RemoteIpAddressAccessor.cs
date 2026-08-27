using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using Umbraco.Extensions;

namespace N3O.Umbraco.Context;

public class RemoteIpAddressAccessor : IRemoteIpAddressAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoteIpAddressAccessor(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public IPAddress GetRemoteIpAddress() {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null) {
            return null;
        } else if (httpContext.Request.IsLocal()) {
            return IPAddress.Loopback;
        }

        return ResolveRemoteIpAddress(httpContext);
    }

    protected virtual IPAddress ResolveRemoteIpAddress(HttpContext httpContext) {
        // Our reverse proxy sets X-Real-IP on every route, replacing whatever the caller sent, so it is the
        // only header here whose value a caller cannot choose. A CDN in front appends to a caller-supplied
        // X-Forwarded-For rather than replacing it, so that header's first entry stays caller-controlled; it
        // is read only when X-Real-IP is absent, which means the request did not pass through the proxy.
        var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();

        if (IPAddress.TryParse(realIp, out var realIpAddress)) {
            return realIpAddress;
        }

        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        var forwardedIp = forwardedFor?.Split(',').FirstOrDefault()?.Trim();

        if (IPAddress.TryParse(forwardedIp, out var forwardedIpAddress)) {
            return forwardedIpAddress;
        }

        return httpContext.Connection.RemoteIpAddress;
    }
}
