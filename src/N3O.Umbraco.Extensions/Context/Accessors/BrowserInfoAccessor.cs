using Microsoft.AspNetCore.Http;
using Umbraco.Extensions;

namespace N3O.Umbraco.Context {
    public class BrowserInfoAccessor : IBrowserInfoAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrowserInfoAccessor(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetUserAgent() {
            var httpContext = _httpContextAccessor.HttpContext;

            var header = httpContext.IfNotNull(x => x.Request.Headers["user-agent"]);

            return header;
        }

        public string GetLanguage() {
            var httpContext = _httpContextAccessor.HttpContext;

            var header = httpContext.IfNotNull(x => x.Request.Headers["accept-language"]);

            return header;
        }
        
        public string GetAccept() {
            var httpContext = _httpContextAccessor.HttpContext;

            var header = (string) httpContext.IfNotNull(x => x.Request.Headers["accept"]);

            return header ?? "*";
        }

    }
}
