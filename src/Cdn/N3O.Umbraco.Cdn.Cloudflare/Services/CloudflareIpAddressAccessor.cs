using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using System.Linq;
using System.Net;

namespace N3O.Umbraco.Cdn.Cloudflare;

public class CloudflareIpAddressAccessor : RemoteIpAddressAccessor {
    public CloudflareIpAddressAccessor(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    protected override IPAddress ResolveRemoteIpAddress(HttpContext httpContext) {
        var header = httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
    
        if (IPAddress.TryParse(header, out var ipAddress)) {
            return ipAddress;
        } else {
            return base.ResolveRemoteIpAddress(httpContext);
        }
    }
}
