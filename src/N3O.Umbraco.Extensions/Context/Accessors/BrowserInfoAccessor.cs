using Microsoft.AspNetCore.Http;

namespace N3O.Umbraco.Context {
    public class BrowserInfoAccessor : IBrowserInfoAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrowserInfoAccessor(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetAccept() {
            return GetHeader("Accept");
        }
        
        public string GetHeader(string headerName) {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null) {
                if (httpContext.Request.Headers.TryGetValue(headerName, out var value)) {
                    return value;
                }
            }

            return null;
        }
        
        public string GetLanguage() {
            var language = GetHeader("Accept-Language");

            if (language.Contains(",")) {
                language = language.Split(",")[0];
            }

            return language;
        }
        
        public string GetUserAgent() {
            return GetHeader("User-Agent");
        }
    }
}
