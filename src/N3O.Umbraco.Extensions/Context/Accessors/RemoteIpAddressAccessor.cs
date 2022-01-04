using Microsoft.AspNetCore.Http;
using System.Net;
using Umbraco.Extensions;

namespace N3O.Umbraco.Context {
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
            var header = httpContext.Request.Headers["X-Forwarded-For"];
        
            if (IPAddress.TryParse(header, out var ipAddress)) {
                return ipAddress;
            } else {
                return httpContext.Connection.RemoteIpAddress;
            }
        }
    }
}
