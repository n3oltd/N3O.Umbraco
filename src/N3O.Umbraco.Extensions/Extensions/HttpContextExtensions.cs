using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions;

public static class HttpContextExtensions {
    public static IPAddress ClientIp(this HttpContext httpContext) {
        if (httpContext.Request.IsLocal()) {
            return IPAddress.Loopback;
        }

        var header = httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ??
                     httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        
        if (IPAddress.TryParse(header, out IPAddress ip)) {
            return ip;
        }

        return httpContext.Connection.RemoteIpAddress;
    }
}
